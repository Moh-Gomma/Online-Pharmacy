using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace pharmace.Helper
{
    public static class DocumentSettings
    {
        private static IWebHostEnvironment _environment;
        private static IConfiguration _configuration;

        public static void Configure(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public static string UploadFile(IFormFile file, string folderName, string newFileName)
        {
            string uploadPath = _configuration["FileUploadSettings:UploadPath"];
            string folderPath = Path.Combine(_environment.ContentRootPath, uploadPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string extension = Path.GetExtension(file.FileName);
            string sanitizedFileName = SanitizeFileName(newFileName);
            string fileName = $"{Guid.NewGuid()}{sanitizedFileName}{extension}";
            string filePath = Path.Combine(folderPath, fileName);

            using var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string uploadPath = _configuration["FileUploadSettings:UploadPath"];
            var filePath = Path.Combine(_environment.ContentRootPath, uploadPath, folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static string UploadDoc(IFormFile file, string folderName)
        {
            string uploadPath = _configuration["FileUploadSettings:UploadPath"];
            string folderPath = Path.Combine(_environment.ContentRootPath, uploadPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);

            using var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);

            return fileName;
        }

        public static void DeleteDoc(string fileName, string folderName)
        {
            string uploadPath = _configuration["FileUploadSettings:UploadPath"];
            var filePath = Path.Combine(_environment.ContentRootPath, uploadPath, folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(fileName, invalidRegStr, "_");
        }
    }
}

