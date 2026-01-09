using Marketplace.Application.IServices.FileServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.Services.FileService
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _env;
        private readonly string _uploadsFolder;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        public FileServices(IHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            var relativePath = configuration["FileUpload:ProductsPath"] ?? "wwwroot/images/products";
            _uploadsFolder = Path.Combine(env.ContentRootPath, relativePath);

            Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<string> UploadProductImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is required");

            if (file.Length > MaxFileSize)
                throw new Exception("File size exceeds 5MB");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new Exception("Invalid file type. Only images allowed");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(_uploadsFolder, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // نرجع URL نسبي عشان الـ Frontend يستخدمه
            return $"/images/products/{fileName}";
        }

        public async Task DeleteImageAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) return;

            var fullPath = Path.Combine(_uploadsFolder, Path.GetFileName(fileUrl));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            await Task.CompletedTask;
        }
    }
}
