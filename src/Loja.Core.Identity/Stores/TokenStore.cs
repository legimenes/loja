using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Data;
using Loja.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Loja.Core.Identity.Stores
{
    public class TokenStore : ITokenStore
    {
        private readonly IdentityDbContext _context;
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public TokenStore(IdentityDbContext context, IdentityErrorDescriber describer = null)
        {
            _context = context;
            ErrorDescriber = describer;
        }

        public async Task<IdentityResult> CreateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));
            
            _context.Add(refreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            RefreshToken refreshToken = await FindByTokenAsync(token, cancellationToken);

            _context.Remove(refreshToken);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteExpiredAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            IEnumerable<RefreshToken> expiredRefreshTokens = await _context.RefreshTokens.Where(rt => rt.ExpirationDate <= DateTime.Now).ToListAsync();
            _context.RemoveRange(expiredRefreshTokens);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<RefreshToken> FindByTokenAsync(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await _context.RefreshTokens.FindAsync(new object[] { token }, cancellationToken);
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
        ~TokenStore()
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