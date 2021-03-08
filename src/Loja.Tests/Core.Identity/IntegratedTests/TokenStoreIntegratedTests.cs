using System;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.Stores;
using Loja.Tests.Core.Identity.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Loja.Tests.Core.Identity.IntegratedTests
{
    public class TokenStoreIntegratedTests : IClassFixture<DbContextFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public TokenStoreIntegratedTests(DbContextFixture dbContextFixture)
        {
            _serviceProvider = dbContextFixture.ServiceProvider;
        }

        [Fact(DisplayName="Create a new refresh token")]
        [Trait("IntegratedTests - Identity", "TokenStore")]
        public async Task CreateRefreshTokenAsync_ShouldCreateANewRefreshToken()
        {
            // Arrange
            ITokenStore tokenStore = _serviceProvider.GetService<ITokenStore>();
            RefreshToken refreshToken = new RefreshToken
            {
                UserId = 3,
                Token = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.Now.AddMinutes(10),
                AccessToken = ""
            };

            // Act
            IdentityResult identityResult = await tokenStore.CreateRefreshTokenAsync(refreshToken, CancellationToken.None);

            // Assert
            Assert.True(identityResult.Succeeded);
        }

        [Fact(DisplayName="Find a refresh token by token")]
        [Trait("IntegratedTests - Identity", "TokenStore")]
        public async Task FindByTokenAsync_ShouldReturnARefreshToken()
        {
            // Arrange
            ITokenStore tokenStore = _serviceProvider.GetService<ITokenStore>();
            string token = "4d527988-42b6-42ed-83fc-7acf5abd4a38";

            // Act
            RefreshToken refreshToken = await tokenStore.FindByTokenAsync(token, CancellationToken.None);

            // Assert
            Assert.NotNull(refreshToken);
        }

        [Fact(DisplayName="Delete a refresh token by token")]
        [Trait("IntegratedTests - Identity", "TokenStore")]
        public async Task DeleteAsync_ShouldDeleteARequestedToken()
        {
            // Arrange
            ITokenStore tokenStore = _serviceProvider.GetService<ITokenStore>();
            string token = "4d527988-42b6-42ed-83fc-7acf5abd4a38";

            // Act
            IdentityResult identityResult = await tokenStore.DeleteAsync(token, CancellationToken.None);

            // Assert
            Assert.True(identityResult.Succeeded);
        }

        [Fact(DisplayName="Delete all expired refresh token")]
        [Trait("IntegratedTests - Identity", "TokenStore")]
        public async Task DeleteExpiredAsync_ShouldDeleteAllExpiredRefreshToken()
        {
            // Arrange
            ITokenStore tokenStore = _serviceProvider.GetService<ITokenStore>();

            // Act
            IdentityResult identityResult = await tokenStore.DeleteExpiredAsync(CancellationToken.None);

            // Assert
            Assert.True(identityResult.Succeeded);
        }
    }
}