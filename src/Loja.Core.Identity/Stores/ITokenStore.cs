using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Loja.Core.Identity.Stores
{
    public interface ITokenStore
    {
        Task<IdentityResult> CreateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<IdentityResult> DeleteAsync(string token, CancellationToken cancellationToken);
        Task<IdentityResult> DeleteExpiredAsync(CancellationToken cancellationToken);
        Task<RefreshToken> FindByTokenAsync(string token, CancellationToken cancellationToken);
    }
}