using GarciaCore.Application.Contracts.ImageResize;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Drawing.Imaging;

namespace GarciaCore.Infrastructure.ImageResize.Local
{
    public class LocalImageResizeService : IImageResizeService
    {
        public void Resize(IFormFile file, string fileName, int height)
        {
            using var image = Image.Load(file.OpenReadStream());
            var coef = image.Height / height;
            image.Mutate(x => x.Resize(image.Width / (image.Height / height), height));
        }
    }
}