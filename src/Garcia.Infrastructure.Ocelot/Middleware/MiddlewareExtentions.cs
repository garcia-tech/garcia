using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using MMLib.SwaggerForOcelot.Configuration;
using Ocelot.Middleware;

namespace Garcia.Infrastructure.Ocelot.Middleware
{
    public static class MiddlewareExtentions
    {
        public static IApplicationBuilder UseGarciaOcelot(this IApplicationBuilder app, Action<CorsPolicyBuilder>? corsPolicyConfigurations = null)
        {
            Action<CorsPolicyBuilder> configurePolicy = corsPolicyConfigurations ??
                new Action<CorsPolicyBuilder>(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithExposedHeaders("Content-Disposition");
                });

            app.UseOcelot().Wait();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(configurePolicy);

            return app;
        }

        public static IApplicationBuilder UseGarciaOcelot(this IApplicationBuilder app, OcelotPipelineConfiguration pipelineConfiguration, Action<CorsPolicyBuilder>? corsPolicyConfigurations = null)
        {
            Action<CorsPolicyBuilder> configurePolicy = corsPolicyConfigurations ??
                new Action<CorsPolicyBuilder>(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithExposedHeaders("Content-Disposition");
                });

            app.UseOcelot(pipelineConfiguration).Wait();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(configurePolicy);
            return app;
        }

        public static IApplicationBuilder UseGarciaSwaggerForOcelotUI(this IApplicationBuilder app, params KeyValuePair<string, string>[] downstreamSwaggerHeaders)
        {
            app.UseSwaggerForOcelotUI(opt =>
            {
                if (downstreamSwaggerHeaders?.Length > 0)
                {
                    opt.DownstreamSwaggerHeaders = downstreamSwaggerHeaders;
                }
            });

            return app;
        }

        public static IApplicationBuilder UseGarciaSwaggerForOcelotUI(this IApplicationBuilder app, Action<SwaggerForOcelotUIOptions> setupAction = null)
        {
            return app.UseSwaggerForOcelotUI(setupAction);
        }
    }
}
