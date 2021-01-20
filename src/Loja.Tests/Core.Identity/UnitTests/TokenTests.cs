using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Loja.Tests.Core.Identity.Fixtures;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Loja.Tests.Core.Identity.UnitTests
{
    public class TokenTests : IClassFixture<TokenFixture>, IClassFixture<RsaFixture>
    {
        private readonly TokenFixture _tokenFixture;
        private readonly RsaFixture _rsaFixture;

        public TokenTests(TokenFixture tokenFixture, RsaFixture rsaFixture)
        {
            _tokenFixture = tokenFixture;
            _rsaFixture = rsaFixture;
        }

        [Fact(DisplayName="Generate a valid access and refresh token")]
        [Trait("UnitTests - Identity", "Token")]
        public async void GenerateToken_ShouldGenerateAValidAccessAndRefreshToken()
        {
            // Arrange
            string issuer = "TheIssuer";
            string[] audience = { "AudienceOne", "AudienceTwo" };
            string userId = "1000";
            string username = "userone";
            double accessTokenExpirationInMinutes = 5;
            double refreshTokenExpirationInMinutes = 10;
            IDictionary<string, object> customClaims = null;
            bool generateRefreshToken = true;

            string privateKey = _rsaFixture.GetPrivateKey();
            _rsaFixture.MockedRsaStore.Setup(m => m.GetPrivateKey(It.IsAny<CancellationToken>())).Returns(Task.FromResult(privateKey));
            RSAParameters rsaParameters = await _rsaFixture.RsaManager.GetRsaPrivateKeyParameters();

            _tokenFixture.MockedRsaManager.Setup(m => m.GetRsaPrivateKeyParameters()).ReturnsAsync(rsaParameters);
            _tokenFixture.MockedTokenStore.Setup(m => m.CreateRefreshTokenAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            Token token = await _tokenFixture.TokenManager.GenerateToken(issuer, audience, userId, username, accessTokenExpirationInMinutes, 
                refreshTokenExpirationInMinutes, customClaims, generateRefreshToken);

            // Assert
            Assert.NotNull(token); 
            Assert.NotNull(token.AccessToken);
            Assert.NotNull(token.RefreshToken);
            _rsaFixture.MockedRsaStore.Verify(m => m.GetPrivateKey(It.IsAny<CancellationToken>()), Times.Once);
            _tokenFixture.MockedRsaManager.Verify(m => m.GetRsaPrivateKeyParameters(), Times.Once);
            _tokenFixture.MockedTokenStore.Verify(m => m.CreateRefreshTokenAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}