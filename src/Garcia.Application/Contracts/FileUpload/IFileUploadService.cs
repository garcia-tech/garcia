using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Garcia.Application.Contracts.FileUpload
{
    public interface IFileUploadService
    {
        /// <summary>
        /// Provides multipart file upload from <see cref="IFormFile"/>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newFileName"></param>
        /// <returns><see cref="UploadedFile"/></returns>
        Task<UploadedFile> MultipartUploadAsync(IFormFile file, string newFileName = null);
        /// <summary>
        /// Provides multipart file upload from <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="originalFileName"></param>
        /// <param name="contentType">It's a mime type of uploading file.</param>
        /// <param name="newFileName"></param>
        /// <returns><see cref="UploadedFile"/></returns>
        Task<UploadedFile> MultipartUploadAsync(Stream stream, string originalFileName, string contentType, string newFileName = null);
        /// <summary>
        /// Gets url from the file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetUrl(string fileName);
        /// <summary>
        /// Gets file name from the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetFileName(string url);
        /// <summary>
        /// Provides base64 upload.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content">The base64 string.</param>
        /// <returns></returns>
        Task<UploadedFile> Base64UploadAsync(string fileName, string content);
    }
}
