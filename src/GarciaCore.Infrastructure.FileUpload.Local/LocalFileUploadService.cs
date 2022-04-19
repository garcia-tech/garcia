using GarciaCore.Application;
using GarciaCore.Application.Contracts.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.FileUpload.Local
{
    public partial class LocalFileUploadService : IFileUploadService
    {
        private LocalFileUploadSettings _settings;

        public LocalFileUploadService(IOptions<LocalFileUploadSettings> settings)
        {
            _settings = settings.Value;
        }

        protected FileWrapper GetDetails(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return new FileWrapper();
            }

            return new FileWrapper(fileName, _settings.BaseUrl, $"{_settings.FileUploadPath}/{fileName}", $"{_settings.BaseUrl}/{_settings.FileUploadPath}/{fileName}", fileName.Split('.').Last());
        }

        public string GetFileName(string url)
        {
            return url.Replace($"{_settings.BaseUrl}/{_settings.FileUploadPath}/", "");
        }

        public string GetUrl(string fileName)
        {
            return GetDetails(fileName).Url;
        }

        public async Task<UploadedFile> MultipartUploadAsync(IFormFile file, string newFileName = null)
        {
            var targetDirectory = _settings.FileUploadPath;
            var fileName = !string.IsNullOrEmpty(newFileName) ? newFileName : $"{Helpers.CreateKey(8)}_{file.FileName}";
            var savePath = Path.Combine(targetDirectory, fileName).Replace("/", "\\");

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return new UploadedFile(file.Name, fileName);
        }

        public async Task<UploadedFile> Base64UploadAsync(string fileName, string content)
        {
            fileName = $"{Helpers.CreateKey(8)}_{fileName}";
            var bytes = Convert.FromBase64String(content);
            var targetDirectory = _settings.FileUploadPath;
            var savePath = Path.Combine(targetDirectory, fileName).Replace("/", "\\");

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
                await fileStream.FlushAsync();
            }

            return new UploadedFile(null, fileName);
        }

        public Task<UploadedFile> MultipartUploadAsync(Stream stream, string originalFileName, string contentType, string newFileName = null)
        {
            throw new NotImplementedException();
        }
    }
}
