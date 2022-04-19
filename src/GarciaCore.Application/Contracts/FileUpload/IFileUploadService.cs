using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Application.Contracts.FileUpload
{
    public interface IFileUploadService
    {
        Task<UploadedFile> MultipartUploadAsync(IFormFile file, string newFileName = null);
        string GetUrl(string fileName);
        string GetFileName(string url);
        Task<UploadedFile> Base64UploadAsync(string fileName, string content);
        Task<UploadedFile> MultipartUploadAsync(Stream stream, string originalFileName, string contentType, string newFileName = null);
    }
}
