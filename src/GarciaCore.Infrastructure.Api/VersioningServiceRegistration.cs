using System;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using GarciaCore.Application;
using GarciaCore.Infrastructure.Api.Providers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace GarciaCore.Infrastructure.Api
{
    public static class VersioningServiceRegistration
    {
        /// <summary>
        /// Configures <see cref="Microsoft.AspNetCore.Mvc.Versioning"/>. Uses <see cref="HeaderApiVersionReader"/> as default version reader and
        /// <see cref="VersioningErrorProvider{TErrorResponse}"/> it takes <typeparamref name="TErrorResponse"/> as type parameter
        /// for creating error responses.
        /// </summary>
        /// <typeparam name="TErrorResponse"><see cref="ApiError"/>
        /// It is a Response model of type ApiError. It determines what kind of response will be returned if an invalid version entered.
        /// </typeparam>
        /// <param name="services"></param>
        /// <param name="headerNames"></param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddApiVersioning<TErrorResponse>(this IServiceCollection services, params string[] headerNames)
            where TErrorResponse : ApiError, new()
        {
            return services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new HeaderApiVersionReader(headerNames);
                opt.ErrorResponses = new VersioningErrorProvider<TErrorResponse>();
            });
        }

        public static IServiceCollection AddApiVersioning(this IServiceCollection services,
            Action<ApiVersioningOptions> options) =>
            services.AddApiVersioning(options);

        /// <summary>
        /// Configures <see cref="Microsoft.AspNetCore.Mvc.Versioning"/>. Uses <see cref="VersioningErrorProvider{TErrorResponse}"/> 
        /// it takes <typeparamref name="TErrorResponse"/> as type parameter
        /// for creating error responses.
        /// </summary>
        /// <typeparam name="TErrorResponse">
        /// It is a Response model of type ApiError. It determines what kind of response will be returned if an invalid version entered.
        ///</typeparam>
        /// <param name="services"></param>
        /// <param name="versionReader"></param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddApiVersioning<TErrorResponse>(this IServiceCollection services, params IApiVersionReader[] versionReader)
            where TErrorResponse : ApiError, new()
        {
            return services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(versionReader);
                opt.ErrorResponses = new VersioningErrorProvider<TErrorResponse>();
            });
        }
        /// <summary>
        /// Adds an API explorer that is API version aware.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="groupNameFormat"></param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddApiVersionedApiExplorer(this IServiceCollection services, string groupNameFormat)
        {
            return services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = groupNameFormat;
                opt.SubstituteApiVersionInUrl = true;
            });
        }
        /// <summary>
        /// Adds an API explorer that is API version aware.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddApiVersionedApiExplorer(this IServiceCollection services, Action<ApiExplorerOptions> options) =>
            services.AddVersionedApiExplorer(options);

        /// <summary>
        /// Provides configuration for versioning in swagger.
        /// </summary>
        /// <param name="services"></param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddApiVersioningSwaggerConfiguration(this IServiceCollection services) =>
            services.ConfigureOptions<SwaggerConfigurationProvider>();

        /// <summary>
        /// Provides configuration for versioning in swagger.
        /// </summary>
        /// <typeparam name="TProvider"></typeparam>
        /// <param name="services"></param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection AddApiVersioningSwaggerConfiguration<TProvider>(this IServiceCollection services)
            where TProvider : SwaggerConfigurationProvider
        {
            services.ConfigureOptions<TProvider>();
            return services;
        }


    }
}
