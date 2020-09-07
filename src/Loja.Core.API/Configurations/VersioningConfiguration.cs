using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Loja.Core.API.Configurations
{
    public static class VersioningConfiguration
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services,
            int defaultMajorVersion, int defaultMinorVersion)
        {
            services.AddApiVersioning(p =>
            {
                p.AssumeDefaultVersionWhenUnspecified = true;
                p.DefaultApiVersion = new ApiVersion(defaultMajorVersion, defaultMinorVersion);
                p.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}