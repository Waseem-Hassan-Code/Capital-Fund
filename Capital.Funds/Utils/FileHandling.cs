using Capital.Funds.Database;
using Capital.Funds.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
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

        public async Task<string> UploadFileAsync(IFormFile fileStream, string ComplaintId)
        {
            try
            {
                // Create the folder if it doesn't exist
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ImageFiles");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine("Folder 'ImageFiles' created.");
                }

                // Generate a unique file name
                var randomFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(fileStream.FileName);
                var targetFilePath = Path.Combine(folderPath, randomFileName);

                // Save the file to the target file path
                using (var fileOutput = System.IO.File.Create(targetFilePath))
                {
                    await fileStream.CopyToAsync(fileOutput);
                }

                // Save file information to the database
                var complaintFiles = new ComplaintFiles
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ComplaintId = ComplaintId,
                    FileName = randomFileName,
                    FileURL = targetFilePath, 
                };

                await _db.ComplaintFiles.AddAsync(complaintFiles);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                {
                    return "File uploaded successfully";
                }
                else
                {
                    return "Failed to save file information to the database";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return "File upload failed";
            }
        }


        public async Task<Stream> GetFileAsStreamAsync(string fileId)
        {
            try
            {
                var file = await _db.ComplaintFiles.FirstOrDefaultAsync(f => f.Id == fileId);

                if (file == null)
                {
                    throw new FileNotFoundException("File not found in database.");
                }

                var filePath = file.FileURL;
                if (!System.IO.File.Exists(filePath))
                {
                    throw new FileNotFoundException("File path does not exist.");
                }

                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving file: {ex.Message}");
                throw new IOException("Error retrieving file.", ex);
            }
        }




        //public async Task<string> UploadFileToDriveAsync(IFormFile file, string complaintId)
        //    {
        //        try
        //        {
        //            if (file == null || file.Length == 0)
        //            {
        //                return "File is empty";
        //            }

        //            if (!IsImage(file.ContentType))
        //            {
        //                return "Invalid file format. Only image files are allowed.";
        //            }

        //            string randomFileName = Guid.NewGuid().ToString("N") + file.FileName;
        //            string fileExtension = Path.GetExtension(file.FileName);

        //            string fileId = await UploadFileToGoogleDrive(file, randomFileName, fileExtension);

        //            if (string.IsNullOrEmpty(fileId))
        //            {
        //                return "Failed to upload file to Google Drive";
        //            }

        //            var complaintFiles = new ComplaintFiles
        //            {
        //                Id = Guid.NewGuid().ToString("N"),
        //                ComplaintId = complaintId,
        //                FileName = randomFileName,
        //                FileURL = fileId,
        //            };
        //            await _db.ComplaintFiles.AddAsync(complaintFiles);
        //            int count = await _db.SaveChangesAsync();

        //            if (count > 0)
        //            {
        //                return "File uploaded successfully";
        //            }

        //            return "File not uploaded";
        //        }
        //        catch (Exception ex)
        //        {
        //            return "Error";
        //        }
        //    }

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

                if (!System.IO.File.Exists(keyFilePath))
                {
                    return null;
                }

                var fileContent = System.IO.File.ReadAllText(keyFilePath);
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



        public void AssignOwnerRole(string fileId, string serviceAccountEmail)
    {
        try
        {
            var driveService = CreateDriveService();

            var permission = new Permission
            {
                Type = "serviceAccount",
                Role = "owner",
                EmailAddress = serviceAccountEmail
            };

            driveService.Permissions.Create(permission, fileId).Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error assigning owner role: {ex.Message}");
            throw;
        }
    }
        public async Task<Stream> ReadImageStream(string fileId)
        {
            Stream memoryStream = null;
            try
            {
                var driveService = CreateDriveService();
                var request = driveService.Files.Get(fileId);
                var file = await request.ExecuteAsync();

                if (file != null)
                {
                    if (file.Size != null)
                    {
                        var downloadUrl = file.WebContentLink;

                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.GetAsync(downloadUrl);

                            if (response.IsSuccessStatusCode)
                            {
                                memoryStream = new MemoryStream();
                                await response.Content.CopyToAsync(memoryStream);
                                memoryStream.Seek(0, SeekOrigin.Begin);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("File size is null.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ReadImageStream: {ex.Message}");
                memoryStream?.Dispose();
                throw;
            }

            return memoryStream;

        }

        //public async Task<string> GetFileLink(string fileId)
        //{
        //    try
        //    {
        //        var driveService = CreateDriveService();

        //        var permissionsRequest = driveService.Permissions.List(fileId);
        //        var permissions = await permissionsRequest.ExecuteAsync();

        //        var authenticatedUserEmail = "tenantsstorage@tenantsstorage.iam.gserviceaccount.com";
        //        AssignOwnerRole(fileId, authenticatedUserEmail);
        //        var hasPermission = permissions.Permissions.Any(p => p.EmailAddress == authenticatedUserEmail);

        //        if (!hasPermission)
        //        {
        //            Console.WriteLine("User does not have permission to access the file.");
        //            return null;
        //        }

        //        var request = driveService.Files.Get(fileId);
        //        var file = await request.ExecuteAsync();

        //        if (file != null)
        //        {
        //            Console.WriteLine($"File ID: {file.Id}");
        //            Console.WriteLine($"WebContentLink: {file.WebContentLink}");

        //            return file.WebContentLink;
        //        }
        //        else
        //        {
        //            Console.WriteLine("File not found.");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in GetFileLink: {ex.Message}");
        //        throw;
        //    }
        //}


        public async Task<bool> DeleteImage(string fileId)
        {
            try
            {
                using (var driveService = CreateDriveService())
                {
                    await driveService.Files.Delete(fileId).ExecuteAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
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
