using System;
using System.Threading;
using System.Threading.Tasks;

namespace Loja.Core.Identity.Stores
{
    public interface IRsaStore : IDisposable
    {
        Task<string> GetPrivateKey(CancellationToken cancellationToken = default(CancellationToken));
        Task<string> GetPublicKey(CancellationToken cancellationToken = default(CancellationToken));
    }
}