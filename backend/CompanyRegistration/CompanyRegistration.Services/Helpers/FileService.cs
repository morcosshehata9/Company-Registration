using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Services.Helpers
{
    public class FileService : IFileService
    {

        private readonly string _webRootPath;

        public FileService(string webRootPath)
        {
            _webRootPath = webRootPath ?? throw new ArgumentNullException(nameof(webRootPath));
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null!;

            folderName = folderName.Replace("..", "").Trim(Path.DirectorySeparatorChar);

            var uploadsBasePath = Path.Combine(_webRootPath, "uploads");

            var finalFolderPath = Path.Combine(uploadsBasePath, folderName);
            Directory.CreateDirectory(finalFolderPath);

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var sanitizedFileName = Path.GetFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(finalFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", folderName, uniqueFileName).Replace("\\", "/");
        }
    }
}
