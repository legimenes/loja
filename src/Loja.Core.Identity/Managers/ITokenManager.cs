using System.Collections.Generic;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;

namespace Loja.Core.Identity.Managers
{
    public interface ITokenManager
    {
        Task<Token> GenerateToken(string issuer, string[] audience, string userId, string username,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes,
            IDictionary<string, object> customClaims = null, bool generateRefreshToken = false);
    }
}