using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
