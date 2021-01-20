using System;
using System.Threading.Tasks;
using Loja.Core.Identity.Data;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Loja.Tests
{
    public class AgileTests
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        public AgileTests()
        {
            string identityConnectionString = "server=localhost;database=lojatemp;user id=sa;password=sasa";
            _services = new ServiceCollection();
            _services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(identityConnectionString));
        }

        [Fact]
        public async Task GetPrivateKey()
        {
            _services.AddScoped<IRsaStore, RsaStore>();
            _services.AddScoped<IRsaManager, RsaManager>();
            
            _serviceProvider = _services.BuildServiceProvider();


            //IRsaStore rsaStore = _serviceProvider.GetRequiredService<IRsaStore>();
            //string privateKey = rsaStore.GetPrivateKey().Result;
            IRsaManager rsaManager = _serviceProvider.GetRequiredService<IRsaManager>();
            //await rsaManager.GetRsaPrivateKeyParameters();
            var keyParam = await rsaManager.GetRsaPublicKeyParameters();
        }
    }
}