using Azure.Storage.Blobs;
using Garcia.Application;
using Garcia.Application.Contracts.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeTypes;

namespace Garcia.Infrastructure.FileUpload.AzureBlob
{
    public class AzureBlobFileUploadService : IFileUploadService
    {
        private readonly AzureBlobSettings _settings;
        private readonly BlobContainerClient _blobContainerClient;

        public AzureBlobFileUploadService(IOptions<AzureBlobSettings> options)
        {
            _settings = options.Value;
            _blobContainerClient = new BlobContainerClient(_settings.ConnectionString, _settings.DefaultContainerName);
        }

        public async Task<UploadedFile> Base64UploadAsync(string fileName, string content)
        {
            fileName = $"{Helpers.CreateKey(8)}_{fileName}";
            var bytes = Convert.FromBase64String(content);
            using var stream = new MemoryStream(bytes);
            var name = MimeTypeMap.GetMimeType(fileName);
            return await CreateResponse(name, fileName, stream);
        }

        public string GetFileName(string url)
        {
            var uriBuilder = new BlobUriBuilder(new Uri(url));
            return uriBuilder.BlobName;
        }

        public string GetUrl(string fileName)
        {
            var blob = _blobContainerClient.GetBlobClient(fileName);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<UploadedFile> MultipartUploadAsync(IFormFile file, string newFileName = null)
        {
            var fileName = !string.IsNullOrEmpty(newFileName) ? newFileName : $"{Helpers.CreateKey(8)}_{file.FileName}";
            var name = file.Name;
            using var streamContent = file.OpenReadStream();
            return await CreateResponse(name, fileName, streamContent);
        }

        public async Task<UploadedFile> MultipartUploadAsync(Stream stream, string originalFileName, string contentType, string newFileName = null)
        {
            var fileName = !string.IsNullOrEmpty(newFileName) ? newFileName : $"{Helpers.CreateKey(8)}_{originalFileName}";
            var name = originalFileName;
            var extention = MimeTypeMap.GetExtension(contentType);
            var uploadedFileName = new GarciaStringBuilder(fileName, extention).ToString();
            return await CreateResponse(name, uploadedFileName, stream);
        }

        private async Task<UploadedFile> CreateResponse(string name, string fileName, Stream stream)
        {
            try
            {
                var result = await _blobContainerClient.UploadBlobAsync(fileName, stream);

                if (result.GetRawResponse().IsError)
                {
                    throw new AzureFailedResponseException(result.GetRawResponse().Status);
                }

                return new UploadedFile(name, fileName);
            }
            catch (Azure.RequestFailedException)
            {
                throw;
            }
        }
    }
}