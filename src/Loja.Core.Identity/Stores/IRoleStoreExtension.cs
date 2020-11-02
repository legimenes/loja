using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Loja.Core.Identity.Stores
{
    public interface IRoleStoreExtension<TRole> : IRoleStore<TRole> where TRole : Role
    {
        Task<IdentityResult> AddMenusAsync(IEnumerable<RoleMenu> roleMenus, CancellationToken cancellationToken);
        Task<IdentityResult> AddPermissionsAsync(IEnumerable<RolePermission> rolePermissions, CancellationToken cancellationToken);
    }
}