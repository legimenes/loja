using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Loja.Tests.Core.Identity.Fixtures;
using Moq;
using Xunit;

namespace Loja.Tests.Core.Identity.UnitTests
{
    public class RsaTests : IClassFixture<RsaFixture>
    {
        private readonly RsaFixture _rsaFixture;

        public RsaTests(RsaFixture rsaFixture)
        {
            _rsaFixture = rsaFixture;
        }

        [Fact(DisplayName="Get RSA private key parameters with a valid private key")]
        [Trait("UnitTests - Identity", "RSA")]
        public async Task GetRsaPrivateKeyParameters_WithAValidPrivateKey_ShouldReturnRsaPrivateKeyParameters()
        {
            // Arrange
            string privateKey = _rsaFixture.GetPrivateKey();
            _rsaFixture.MockedRsaStore.Setup(m => m.GetPrivateKey(It.IsAny<CancellationToken>())).Returns(Task.FromResult(privateKey));

            // Act
            RSAParameters rsaParameters = await _rsaFixture.RsaManager.GetRsaPrivateKeyParameters();

            // Assert
            Assert.NotNull(rsaParameters.D);
            Assert.NotNull(rsaParameters.DP);
            Assert.NotNull(rsaParameters.DQ);
            Assert.NotNull(rsaParameters.Exponent);
            Assert.NotNull(rsaParameters.InverseQ);
            Assert.NotNull(rsaParameters.Modulus);
            Assert.NotNull(rsaParameters.P);
            Assert.NotNull(rsaParameters.Q);
            _rsaFixture.MockedRsaStore.Verify(m => m.GetPrivateKey(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName="Get RSA public key parameters with a valid public key")]
        [Trait("UnitTests - Identity", "RSA")]
        public async Task GetRsaPublicKeyParameters_WithAValidPublicKey_ShouldReturnRsaPublicKeyParameters()
        {
            // Arrange
            string publicKey = _rsaFixture.GetPublicKey();

           _rsaFixture.MockedRsaStore.Setup(m => m.GetPublicKey(It.IsAny<CancellationToken>())).Returns(Task.FromResult(publicKey));

            // Act
            RSAParameters rsaParameters = await _rsaFixture.RsaManager.GetRsaPublicKeyParameters();

            // Assert
            Assert.NotNull(rsaParameters.Exponent);
            Assert.NotNull(rsaParameters.Modulus);
            _rsaFixture.MockedRsaStore.Verify(m => m.GetPublicKey(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}