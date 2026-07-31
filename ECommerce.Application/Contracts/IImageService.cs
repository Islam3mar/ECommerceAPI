using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Contracts
{
    public interface IImageService
    {
        // Saves the image inside Files/Images/{folder}/{generated file name} and returns the RELATIVE path
        // in the same shape already stored in Product.PictureUrl (e.g. "Images/Products/guid.jpg"),
        // so PictureUrlResolver can turn it into a full URL later exactly like the seeded products.
        Task<string> SaveImageAsync(IFormFile file, string folder, CancellationToken ct = default);

        // Deletes a previously saved image using the relative path returned by SaveImageAsync.
        void DeleteImage(string relativePath);
    }
}
