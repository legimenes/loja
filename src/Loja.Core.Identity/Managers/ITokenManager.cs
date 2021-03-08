using System.Collections.Generic;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;

namespace Loja.Core.Identity.Managers
{
    public interface ITokenManager
    {
        Task<Token> GenerateToken(string token, string issuer, string[] audiences, double accessTokenExpirationInMinutes,
            double refreshTokenExpirationInMinutes, IDictionary<string, object> customClaims = null,
            bool generateRefreshToken = false);
        Task<Token> GenerateToken(string issuer, string[] audiences, string userId, string username,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes,
            IDictionary<string, object> customClaims = null, bool generateRefreshToken = false);
    }
}