using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folder, CancellationToken ct = default)
        {
            // Physical folder on disk: {ContentRoot}/Files/Images/{folder}
            // Matches exactly where Program.cs mounts UseStaticFiles from ("Files") + the "Images" prefix
            // that PictureUrlResolver already assumes for every seeded product.
            var physicalFolder = Path.Combine(_env.ContentRootPath, "Files", "Images", folder);

            if (!Directory.Exists(physicalFolder))
                Directory.CreateDirectory(physicalFolder);

            // Guid file name => avoids collisions/overwrites and strips any unsafe characters from the original name.
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var physicalPath = Path.Combine(physicalFolder, fileName);

            await using var stream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            // Relative path stored in Product.PictureUrl, e.g. "Images/Products/3f2c...png"
            return Path.Combine("Images", folder, fileName).Replace("\\", "/");
        }

        public void DeleteImage(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            var physicalPath = Path.Combine(_env.ContentRootPath, "Files", relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);
        }
    }
}
