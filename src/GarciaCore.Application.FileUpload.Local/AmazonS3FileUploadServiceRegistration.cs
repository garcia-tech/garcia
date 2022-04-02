using GarciaCore.Application.Contracts.FileUpload;
using GarciaCore.Application.Contracts.Marketing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GarciaCore.Application.FileUpload.Local
{
    public static class LocalFileUploadServiceRegistration
    {
        public static IServiceCollection RegisterLocalFileUploadService(this IServiceCollection services, LocalFileUploadSettings settings)
        {
            services.Configure<LocalFileUploadSettings>(options =>
            {
                options.BaseUrl = settings.BaseUrl;
                options.FileUploadPath = settings.FileUploadPath;
            });

            services.AddScoped<IFileUploadService, LocalFileUploadService>();
            return services;
        }

        public static IServiceCollection RegisterLocalFileUploadService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LocalFileUploadSettings>(options =>
            {
                options.BaseUrl = configuration[$"{nameof(LocalFileUploadSettings)}:{nameof(options.BaseUrl)}"];
                options.FileUploadPath = configuration[$"{nameof(LocalFileUploadSettings)}:{nameof(options.FileUploadPath)}"];
            });

            services.AddScoped<IFileUploadService, LocalFileUploadService>();
            return services;
        }
    }
}
