using Loja.Core.Identity.Data;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Loja.Tests.Core.Identity.Fixtures
{
    public class TokenStoreFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public TokenStoreFixture()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            string identityConnectionString = "Server=localhost;Database=LOJATEMP;User Id=sa;Password=sasa";

            Mock<IConfigurationSection> mockedConfigurationSection = new Mock<IConfigurationSection>();
            mockedConfigurationSection.SetupGet(m => m[It.Is<string>(s => s == "IdentityConnection")]).Returns(identityConnectionString);

            Mock<IConfiguration> mockedConfiguration = new Mock<IConfiguration>();
            mockedConfiguration.Setup(x => x.GetSection(It.Is<string>(s=>s == "ConnectionStrings"))).Returns(mockedConfigurationSection.Object);
            //mockedConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:SecretKey")]).Returns("e61b1179-d442-401e-991a-57be9995f253");
            IConfiguration configuration = mockedConfiguration.Object;

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            //services.AddDefaultIdentity<User>()
            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddUserManager<AppUserManager<User>>()
                .AddRoleManager<AppRoleManager<Role>>();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<ITokenStore, TokenStore>();
        }

        public void Dispose() { }
    }
}