using Garcia.Application.Contracts.FileUpload;
using Garcia.Application.Contracts.Marketing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.FileUpload.Local
{
    public static class LocalFileUploadServiceRegistration
    {
        public static IServiceCollection AddLocalFileUploadService(this IServiceCollection services, LocalFileUploadSettings settings)
        {
            services.Configure<LocalFileUploadSettings>(options =>
            {
                options.BaseUrl = settings.BaseUrl;
                options.FileUploadPath = settings.FileUploadPath;
            });

            services.AddScoped<IFileUploadService, LocalFileUploadService>();
            return services;
        }

        public static IServiceCollection AddLocalFileUploadService(this IServiceCollection services, IConfiguration configuration)
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
