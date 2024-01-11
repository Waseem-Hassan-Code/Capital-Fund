using Capital.Funds.Database;
using Capital.Funds.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Capital.Funds.Utils
{
    public class FileHandling
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDb _db;

        public FileHandling(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string complaintId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return "File is empty";
                }

                var attachmentsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Attachments");
                if (!Directory.Exists(attachmentsFolder))
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }
                string randomFileName = Guid.NewGuid().ToString("N");
                string fileExtension = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(attachmentsFolder, $"{randomFileName}{fileExtension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    var complaintFiles = new ComplaintFiles
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ComplaintId = complaintId,
                        FileName = randomFileName,
                        FileURL = filePath,
                    };

                    await _db.ComplaintFiles.AddAsync(complaintFiles);

                    int count = await _db.SaveChangesAsync();
                    if(count > 0)
                    {
                        await file.CopyToAsync(stream);
                    }
                    return "File uploaded successfully";
                }

                return "File not uploaded"; 
            }
            catch (Exception ex)
            {
                return $"Error uploading file: {ex.Message}";
            }
        }
    }
}
