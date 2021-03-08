using Loja.Core.API.Configurations;
using Loja.Core.Identity.Configurations;
using Loja.Identity.API.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loja.Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            VersioningConfiguration.AddConfiguration(services, 1, 0);
            SwaggerConfiguration.AddConfiguration(services);
            IdentityConfiguration.AddConfiguration(services,  Configuration.GetConnectionString("IdentityConnection"));
            ApiConfiguration.AddConfiguration(services);
            DependencyInjectionConfiguration.RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ApiConfiguration.UseConfiguration(app, env);
            SwaggerConfiguration.UseConfiguration(app, provider);
        }
    }
}