using Amazon.S3;
using Amazon.S3.Model;
using GarciaCore.Application;
using GarciaCore.Application.Contracts.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GarciaCore.Infrastructure.FileUpload.AmazonS3
{
    public class AmazonS3FileUplaodService : IFileUploadService
    {
        private AmazonS3Settings _settings;
        private readonly AmazonS3Client s3Client;

        public AmazonS3FileUplaodService(IOptions<AmazonS3Settings> settings)
        {
            _settings = settings.Value;
            s3Client = new AmazonS3Client(_settings.AccessKeyId, _settings.SecretAccessKey, Amazon.RegionEndpoint.EUCentral1);
        }

        public FileWrapper GetDetails(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return new FileWrapper();
            }

            return new FileWrapper(fileName, _settings.BucketUrl, $"{_settings.BucketUrl}/{fileName}", $"{_settings.BucketUrl}/{fileName}", fileName.Split('.').Last());
        }

        public async Task<UploadedFile> MultipartUploadAsync(IFormFile file)
        {
            var fileName = $"{Helpers.CreateKey(8)}_{file.FileName}";
            var name = file.Name;

            using (Stream fileToUpload = file.OpenReadStream())
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = _settings.BucketName;
                putObjectRequest.Key = fileName;
                putObjectRequest.InputStream = fileToUpload;
                putObjectRequest.ContentType = file.ContentType;
                var response = await s3Client.PutObjectAsync(putObjectRequest);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? new UploadedFile(name, fileName) : null;
            }
        }

        public async Task<UploadedFile> MultipartUploadAsync(Stream stream, string originalFileName, string contentType, string newFileName = null)
        {
            var fileName = !string.IsNullOrEmpty(newFileName) ? newFileName : $"{Helpers.CreateKey(8)}_{originalFileName}";
            var name = originalFileName;
            var putObjectRequest = new PutObjectRequest();
            putObjectRequest.BucketName = _settings.BucketName;
            putObjectRequest.Key = fileName;
            putObjectRequest.InputStream = stream;
            putObjectRequest.ContentType = contentType;
            var response = await s3Client.PutObjectAsync(putObjectRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? new UploadedFile(name, fileName) : null;
        }

        public string GetUrl(string fileName)
        {
            return GetDetails(fileName).Url;
        }

        private string GeneratePreSignedURL(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = objectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(24)
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }

        public string GetFileName(string url)
        {
            return url.Replace($"{_settings.BucketUrl}/", "");
        }

        public async Task<UploadedFile> Base64UploadAsync(string fileName, string content)
        {
            fileName = $"{Helpers.CreateKey(8)}_{fileName}";
            var bytes = Convert.FromBase64String(content);

            using (Stream fileToUpload = new MemoryStream(bytes))
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = _settings.BucketName;
                putObjectRequest.Key = fileName;
                putObjectRequest.InputStream = fileToUpload;
                var response = await s3Client.PutObjectAsync(putObjectRequest);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? new UploadedFile(null, fileName) : null;
            }
        }
    }
}
