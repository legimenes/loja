using System.Threading.Tasks;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.Stores;
using Loja.Tests.Core.Identity.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Loja.Tests.Core.Identity.IntegratedTests
{
    public class UserIntegratedTests : IClassFixture<DbContextFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public UserIntegratedTests(DbContextFixture dbContextFixture)
        {
            _serviceProvider = dbContextFixture.ServiceProvider;
        }

        [Fact(DisplayName="Reset user password")]
        [Trait("UserIntegratedTests - Identity", "User")]
        public async Task ResetPasswordAsync_ShouldResetUserPassword()
        {
            // Arrange
            AppUserManager<User> userManager = _serviceProvider.GetService<AppUserManager<User>>();

            // Act
            //IdentityResult identityResult = await tokenStore.CreateRefreshTokenAsync(refreshToken, CancellationToken.None);
            User user = await userManager.FindByNameAsync("user1");
            string code = await userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await userManager.ResetPasswordAsync(user, code, "Test@1");

            // Assert
            Assert.True(identityResult.Succeeded);
        }
    }
}