using Garcia.Application.Contracts.FileUpload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.FileUpload.AzureBlob
{
    public static class AzureBlobFileUploadRegistration
    {
        public static IServiceCollection AddAzureBlobFileUpload(this IServiceCollection services, AzureBlobSettings settings)
        {
            services.Configure<AzureBlobSettings>(options =>
            {
                options.ConnectionString = settings.ConnectionString;
                options.DefaultContainerName = settings.DefaultContainerName;
            });

            services.AddScoped<IFileUploadService, AzureBlobFileUploadService>();
            return services;
        }

        public static IServiceCollection AddAzureBlobFileUpload(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureBlobSettings>(options =>
            {
                options.ConnectionString = configuration[$"{nameof(AzureBlobSettings)}:{nameof(options.ConnectionString)}"];
                options.DefaultContainerName = configuration[$"{nameof(AzureBlobSettings)}:{nameof(options.DefaultContainerName)}"];
            });

            services.AddScoped<IFileUploadService, AzureBlobFileUploadService>();
            return services;
        }
    }
}
