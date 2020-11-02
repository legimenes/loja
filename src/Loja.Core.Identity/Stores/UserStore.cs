using System;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Data;
using Loja.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Loja.Core.Identity.Stores
{
    public class UserStore : IUserStoreExtension<User>, IUserPasswordStore<User>, IUserSecurityStampStore<User>
    {
        private readonly IdentityDbContext _context;

        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public UserStore(IdentityDbContext context, IdentityErrorDescriber describer = null)
        {
            _context = context;
            ErrorDescriber = describer;
        }

        #region IUserStore

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _context.Add(user);
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            user.CreationDate = DateTime.Now;
            user.Active = true;
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Remove(user);
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
        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (long.TryParse(userId, out long id))
            {
                return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            }                
            else
            {
                return await Task.FromResult((User)null);
            }
        }
        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return _context.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper() == normalizedUserName.ToUpper(), cancellationToken);
        }
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName.ToUpper());
        }
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Id.ToString());
        }
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }
        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.CompletedTask;
        }
        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.UserName = userName;
            return Task.CompletedTask;
        }
        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Attach(user);
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            _context.Update(user);
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

        #endregion

        #region IUserStoreExtension

        public async Task<IdentityResult> DeactivateAsync(long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            User user = await FindByIdAsync(userId.ToString());
            _context.Attach(user);
            user.Active = false;
            _context.Update(user);

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

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserSecurityStampStore

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (stamp == null)
            {
                throw new ArgumentNullException(nameof(stamp));
            }
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        #endregion

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
        ~UserStore()
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