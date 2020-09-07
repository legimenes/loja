using Loja.Cadastros.API.Configurations;
using Loja.Core.API.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loja.Cadastros.API
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
            DependencyInjectionConfiguration.RegisterServices(services);
            ApiConfiguration.AddConfiguration(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ApiConfiguration.UseConfiguration(app, env);
        }
    }
}