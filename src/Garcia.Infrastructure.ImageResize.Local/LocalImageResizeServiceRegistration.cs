using Garcia.Application;
using Garcia.Application.Contracts.ImageResize;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garcia.Infrastructure.ImageResize.Local
{
    public static class LocalImageResizeServiceRegistration
    {
        public static IServiceCollection AddLocalImageResizeService(this IServiceCollection services, ImageResizeSettings settings)
        {
            services.Configure<ImageResizeSettings>(options =>
            {
                options.MaximumHeight = settings.MaximumHeight;
                options.DefaultHeight = settings.DefaultHeight;
                options.PreserveOriginalFile = settings.PreserveOriginalFile;
                options.ResizedFileSuffix = settings.ResizedFileSuffix;
            });

            services.AddScoped<IImageResizeService, LocalImageResizeService>();
            return services;
        }

        public static IServiceCollection AddLocalImageResizeService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ImageResizeSettings>(options =>
            {
                options.MaximumHeight = int.Parse(configuration[$"{nameof(ImageResizeSettings)}:{nameof(options.MaximumHeight)}"]);
                options.DefaultHeight = int.Parse(configuration[$"{nameof(ImageResizeSettings)}:{nameof(options.DefaultHeight)}"]);
                options.PreserveOriginalFile = bool.Parse(configuration[$"{nameof(ImageResizeSettings)}:{nameof(options.PreserveOriginalFile)}"]);
                options.ResizedFileSuffix = configuration[$"{nameof(ImageResizeSettings)}:{nameof(options.ResizedFileSuffix)}"];
            });

            services.AddScoped<IImageResizeService, LocalImageResizeService>();
            return services;
        }
    }
}