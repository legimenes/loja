using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Loja.Core.Identity.Stores
{
    public interface IUserStoreExtension<TUser> : IUserStore<TUser> where TUser : User
    {
        Task<IdentityResult> DeactivateAsync(long userId, CancellationToken cancellationToken);
    }
}