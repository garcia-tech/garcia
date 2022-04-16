using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GarciaCore.Application.Contracts.ImageResize
{
    public interface IImageResizeService
    {
        void Resize(IFormFile file, string fileName, int height);
    }
}
