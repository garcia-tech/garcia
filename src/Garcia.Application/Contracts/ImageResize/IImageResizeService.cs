using System.IO;
using Microsoft.AspNetCore.Http;

namespace Garcia.Application.Contracts.ImageResize
{
    public interface IImageResizeService
    {
        ImageResizeSettings ImageResizeSettings { get; set; }

        /// <summary>
        /// Resizes image to the newHeight value. Default height will be used if newHeight is not provided.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        Stream ResizeToStream(IFormFile file, int? newHeight = null);
    }
}
