using GarciaCore.Application;
using GarciaCore.Application.Contracts.ImageResize;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.Drawing.Imaging;

namespace GarciaCore.Infrastructure.ImageResize.Local
{
    public class LocalImageResizeService : IImageResizeService
    {
        private readonly ImageResizeSettings _imageResizeSettings;

        public LocalImageResizeService(IOptions<ImageResizeSettings> options)
        {
            _imageResizeSettings = options.Value;
        }

        public Stream ResizeToStream(IFormFile file, int? newHeight = null)
        {
            if (!newHeight.HasValue)
            {
                newHeight = _imageResizeSettings.DefaultHeight;
            }

            IImageFormat format;
            var stream = new MemoryStream();
            using var image = Image.Load(file.OpenReadStream(), out format);
            image.Mutate(x => x.Resize(image.Width / (image.Height / newHeight.Value), newHeight.Value));
            image.Save(stream, format);
            return stream;
        }
    }
}