using System.Threading.Tasks;
using Loja.Core.Identity.Models;

namespace Loja.Core.Identity.OAuth
{
    public interface IOAuthService
    {
         Task<Token> GetTokenAsync(TokenRequest tokenRequest, string issuer, string[] audiences,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes);
    }
}