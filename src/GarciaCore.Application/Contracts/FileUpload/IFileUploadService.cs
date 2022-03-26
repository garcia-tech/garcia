using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Application.Contracts.FileUpload
{
    public interface IFileUploadService
    {
        Task<string> MultipartUploadAsync(IFormFile file);
        string GetUrl(string fileName);
        string GetFileName(string url);
        Task<string> Base64UploadAsync(string fileName, string content);
    }
}
