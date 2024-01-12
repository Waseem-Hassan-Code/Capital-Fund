using Capital.Funds.Database;
using Capital.Funds.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Capital.Funds.Utils
{
    public class FileHandling
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDb _db;

        public FileHandling(IWebHostEnvironment webHostEnvironment, ApplicationDb db)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string complaintId)
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

                string randomFileName = Guid.NewGuid().ToString("N")+file.FileName;
                string fileExtension = Path.GetExtension(file.FileName);
                string baseFolder = "Attachments";
                string relativePath = Path.Combine(baseFolder, $"{randomFileName}{fileExtension}");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var complaintFiles = new ComplaintFiles
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ComplaintId = complaintId,
                    FileName = randomFileName,
                    FileURL = relativePath, 
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
                return $"Error uploading file: {ex.Message}";
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
