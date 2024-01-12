using Capital.Funds.Database;
using Capital.Funds.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using ImageMagick;
using System.Net;

namespace Capital.Funds.Utils
{
    public class FileHandling
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDb _db;
        private DriveService _driveService;
        public static string googleDriveAPIKey = Path.Combine("GoogleApiKey", "Key.json");

        public FileHandling(IWebHostEnvironment webHostEnvironment, ApplicationDb db)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public async Task<string> UploadFileToDriveAsync(IFormFile file, string complaintId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return "File is empty";
                }

                if (!IsImage(file.ContentType))
                {
                    return "Invalid file format. Only image files are allowed.";
                }

                string randomFileName = Guid.NewGuid().ToString("N") + file.FileName;
                string fileExtension = Path.GetExtension(file.FileName);

                string fileId = await UploadFileToGoogleDrive(file, randomFileName, fileExtension);

                if (string.IsNullOrEmpty(fileId))
                {
                    return "Failed to upload file to Google Drive";
                }

                var complaintFiles = new ComplaintFiles
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ComplaintId = complaintId,
                    FileName = randomFileName,
                    FileURL = fileId,
                };
                await _db.ComplaintFiles.AddAsync(complaintFiles);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                {
                    return "File uploaded successfully";
                }

                return "File not uploaded";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        private async Task<string> UploadFileToGoogleDrive(IFormFile file, string fileName, string fileExtension)
        {
            try
            {
                byte[] compressedData = CompressImage(file);

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    Parents = new List<string> { "1a8oOnQ5VpommyI6rBXEB8grtrOiAy4QK" },
                };


                using (var inMemoryStream = new MemoryStream(compressedData))
                {
                    using (var driveService = CreateDriveService())
                    {
                        var request = driveService.Files.Create(fileMetadata, inMemoryStream, file.ContentType);
                        request.Fields = "id";
                        await request.UploadAsync();

                        var fileUploaded = request.ResponseBody;

                        return fileUploaded?.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "Error";
            }
        }

        private DriveService CreateDriveService()
        {
            GoogleCredential credential;

            try
            {
                var keyFilePath = Path.Combine(Directory.GetCurrentDirectory(), "GoogleApiKey", "Key.json");

                if (!File.Exists(keyFilePath))
                {
                    return null;
                }

                var fileContent = File.ReadAllText(keyFilePath);
                using (var stream = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(DriveService.ScopeConstants.Drive);
                }

                Google.Apis.Util.Store.FileDataStore logger = new Google.Apis.Util.Store.FileDataStore("Google.Apis.Requests.RequestsResponse");

                _driveService = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Tenant Complaints",
                });

                return _driveService;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating DriveService: {ex.Message}");
                throw; 
            }
        }

        public async Task<byte[]> ReadImageStream(string fileId)
        {
            try
            {
                using (var driveService = CreateDriveService())
                {
                    var fileContent = await driveService.Files.Get(fileId).ExecuteAsStreamAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        await fileContent.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task DeleteImage(string fileId)
        {
            try
            {
                using (var driveService = CreateDriveService())
                {
                    await driveService.Files.Delete(fileId).ExecuteAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private byte[] CompressImage(IFormFile file)
        {
            using (var inputStream = file.OpenReadStream())
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var image = new MagickImage(inputStream))
                    {
                        image.Quality = 60;
                        image.Write(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        private bool IsImage(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return false;
            }

            string[] allowedImageTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/webp" };

            return allowedImageTypes.Contains(contentType.ToLower());
        }
    }
}
