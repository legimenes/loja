using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Data;
using Loja.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Loja.Core.Identity.Stores
{
    public class RoleStore : IRoleStoreExtension<Role>
    {
        private readonly IdentityDbContext _context;

        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public RoleStore(IdentityDbContext context, IdentityErrorDescriber describer = null)
        {
            _context = context;
            ErrorDescriber = describer;
        }

        #region IRoleStore

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            _context.Add(role);
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Remove(role);
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

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (long.TryParse(roleId, out long id))
            {
                return await _context.Roles.FindAsync(new object[] { id }, cancellationToken);
            }                
            else
            {
                return await Task.FromResult((Role)null);
            }
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return _context.Roles.FirstOrDefaultAsync(u => u.Name.ToUpper() == normalizedRoleName.ToUpper(), cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name.ToUpper());
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Attach(role);
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            _context.Update(role);
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

        #region IRoleStoreExtension

        public async Task<IdentityResult> AddMenusAsync(IEnumerable<RoleMenu> roleMenus, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (roleMenus == null)
            {
                throw new ArgumentNullException(nameof(roleMenus));
            }

            foreach (RoleMenu roleMenu in roleMenus)
            {
                _context.RoleMenus.Add(roleMenu);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> AddPermissionsAsync(IEnumerable<RolePermission> rolePermissions, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (rolePermissions == null)
            {
                throw new ArgumentNullException(nameof(rolePermissions));
            }

            foreach (RolePermission rolePermission in rolePermissions)
            {
                _context.RolePermissions.Add(rolePermission);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
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
        ~RoleStore()
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