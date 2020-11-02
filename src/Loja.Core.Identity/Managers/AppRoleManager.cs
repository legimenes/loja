using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Loja.Core.Identity.Managers
{
    public class AppRoleManager<TRole> : RoleManager<TRole> where TRole : Role
    {
        private readonly IRoleStoreExtension<TRole> _storeExtension;

        public AppRoleManager(IRoleStoreExtension<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger) 
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _storeExtension = store;
        }

        public async Task<IdentityResult> AddMenuAsync(long roleId, long menuId)
        {
            ThrowIfDisposed();
            if (roleId == 0)
            {
                throw new ArgumentNullException(nameof(roleId));
            }
            if (menuId == 0)
            {
                throw new ArgumentNullException(nameof(menuId));
            }

            RoleMenu roleMenu = new RoleMenu
            {  
                RoleId = roleId,
                MenuId = menuId
            };

            return await AddMenusAsync(new RoleMenu[] { roleMenu });
        }

        public async Task<IdentityResult> AddMenusAsync(IEnumerable<RoleMenu> roleMenus)
        {
            ThrowIfDisposed();
            if (roleMenus == null)
            {
                throw new ArgumentNullException(nameof(roleMenus));
            }

            return await _storeExtension.AddMenusAsync(roleMenus, CancellationToken.None);
        }

        public async Task<IdentityResult> AddPermissionAsync(long roleId, long permissionId, string value1)
        {
            ThrowIfDisposed();
            if (roleId == 0)
            {
                throw new ArgumentNullException(nameof(roleId));
            }
            if (permissionId == 0)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }

            RolePermission rolePermission = new RolePermission
            {  
                RoleId = roleId,
                PermissionId = permissionId,
                Value1 = string.IsNullOrWhiteSpace(value1) ? null : value1
            };

            return await AddPermissionsAsync(new RolePermission[] { rolePermission });
        }

        public async Task<IdentityResult> AddPermissionsAsync(IEnumerable<RolePermission> rolePermissions)
        {
            ThrowIfDisposed();
            if (rolePermissions == null)
            {
                throw new ArgumentNullException(nameof(rolePermissions));
            }

            return await _storeExtension.AddPermissionsAsync(rolePermissions, CancellationToken.None);
        }
    }
}