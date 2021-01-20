using System;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Data;
using Loja.Core.Identity.Models;

namespace Loja.Core.Identity.Stores
{
    public class RsaStore : IRsaStore
    {
        private readonly IdentityDbContext _context;

        public RsaStore(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetPrivateKey(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            RsaKey rsaKey = await GetRsaKey();

            return rsaKey.PrivateKey;
        }

        public async Task<string> GetPublicKey(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            RsaKey rsaKey = await GetRsaKey();

            return rsaKey.PublicKey;
        }

        private async Task<RsaKey> GetRsaKey()
        {
            return await _context.RsaKey;
        }

        #region IDisposable
        
        private bool _disposed = false;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }

                _disposed = true;
            }
        }
        ~RsaStore()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}