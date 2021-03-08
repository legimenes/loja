using System;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation.AspNetCore;
using Loja.Core.API.Behaviors;
using Loja.Core.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Loja.Identity.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            Assembly.GetAssembly(typeof(Core.Identity.OAuth.TokenRequestValidator));
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(assemblies));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = InvalidModelStateResponse.SetModelStateResponse;
            });

            return services;
        }

        public static IApplicationBuilder UseConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}