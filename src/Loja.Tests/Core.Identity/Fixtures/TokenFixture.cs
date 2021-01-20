using System;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Stores;
using Moq;

namespace Loja.Tests.Core.Identity.Fixtures
{
    public class TokenFixture : IDisposable
    {
        public readonly Mock<ITokenStore> MockedTokenStore;
        public readonly Mock<IRsaManager> MockedRsaManager;
        public readonly ITokenManager TokenManager;

        public TokenFixture()
        {
            MockedTokenStore = new Mock<ITokenStore>();
            MockedRsaManager = new Mock<IRsaManager>();
            TokenManager = new TokenManager(MockedRsaManager.Object, MockedTokenStore.Object);
        }

        public void Dispose() { }
    }
}