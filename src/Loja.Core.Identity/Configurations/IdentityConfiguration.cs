using Loja.Core.Identity.Data;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.OAuth;
using Loja.Core.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Loja.Core.Identity.Configurations
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            //services.AddDefaultIdentity<User>()
            //  .AddRoles<Role>()
            services.AddIdentity<User, Role>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddUserManager<AppUserManager<User>>()
                .AddRoleManager<AppRoleManager<Role>>();

            services.AddScoped<IUserStoreExtension<User>, UserStore>();
            services.AddScoped<IRoleStoreExtension<Role>, RoleStore>();
            services.AddScoped<IRsaStore, RsaStore>();
            services.AddScoped<ITokenStore, TokenStore>();

            services.AddScoped<IRsaManager, RsaManager>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IOAuthService, OAuthService>();

            return services;
        }
    }
}