using Garcia.Application;
using Garcia.Application.Contracts.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeTypes;

namespace Garcia.Infrastructure.FileUpload.Local
{
    public partial class LocalFileUploadService : IFileUploadService
    {
        private readonly LocalFileUploadSettings _settings;

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

        public async Task<UploadedFile> MultipartUploadAsync(Stream stream, string originalFileName, string contentType, string newFileName = null)
        {
            var info = new DirectoryInfo(_settings.FileUploadPath);

            if (!info.Exists) info.Create();

            var fileName = !string.IsNullOrEmpty(newFileName) ? newFileName : $"{Helpers.CreateKey(8)}_{originalFileName}";
            var extention = MimeTypeMap.GetExtension(contentType);
            var path = Path.Combine(_settings.FileUploadPath, fileName, extention);
            using var outputFileStream = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(outputFileStream);
            return new UploadedFile(originalFileName, fileName);
        }
    }
}
