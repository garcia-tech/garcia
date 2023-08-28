using System.Reflection;
using Amazon;
using Garcia.Application.Contracts.FileUpload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.FileUpload.AmazonS3
{
    public static class AmazonS3FileUploadServiceRegistration
    {
        public static IServiceCollection AddAmazonS3FileUploadService(this IServiceCollection services, AmazonS3Settings settings)
        {
            services.Configure<AmazonS3Settings>(options =>
            {
                options.AccessKeyId = settings.AccessKeyId;
                options.BucketName = settings.BucketName;
                options.BucketUrl = settings.BucketUrl;
                options.SecretAccessKey = settings.SecretAccessKey;
                options.ServiceUrl = settings.ServiceUrl;
                options.RegionEndpoint = settings.RegionEndpoint;
            });

            services.AddScoped<IFileUploadService, AmazonS3FileUplaodService>();
            return services;
        }

        public static IServiceCollection AddAmazonS3FileUploadService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AmazonS3Settings>(options =>
            {
                options.AccessKeyId = configuration[$"{nameof(AmazonS3Settings)}:{nameof(options.AccessKeyId)}"];
                options.BucketName = configuration[$"{nameof(AmazonS3Settings)}:{nameof(options.BucketName)}"];
                options.BucketUrl = configuration[$"{nameof(AmazonS3Settings)}:{nameof(options.BucketUrl)}"];
                options.SecretAccessKey = configuration[$"{nameof(AmazonS3Settings)}:{nameof(options.SecretAccessKey)}"];
                options.ServiceUrl = configuration[$"{nameof(AmazonS3Settings)}:{nameof(options.ServiceUrl)}"];
            });

            services.AddScoped<IFileUploadService, AmazonS3FileUplaodService>();
            return services;
        }
    }
}
