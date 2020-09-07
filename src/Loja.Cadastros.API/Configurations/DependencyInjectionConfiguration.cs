using Loja.Core.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Loja.Cadastros.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
        }
    }
}