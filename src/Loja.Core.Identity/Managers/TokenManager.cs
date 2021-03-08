using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Loja.Core.Identity.Managers
{
    public class TokenManager : ITokenManager
    {
        private readonly IRsaManager _rsaManager;
        private readonly ITokenStore _tokenStore;

        public TokenManager(IRsaManager rsaManager, ITokenStore tokenStore)
        {
            _rsaManager = rsaManager;
            _tokenStore = tokenStore;
        }

        public async Task<Token> GenerateToken(string token, string issuer, string[] audiences, double accessTokenExpirationInMinutes,
            double refreshTokenExpirationInMinutes, IDictionary<string, object> customClaims = null, bool generateRefreshToken = false)
        {
            await _tokenStore.DeleteExpiredAsync(CancellationToken.None);
            RefreshToken refreshToken = await _tokenStore.FindByTokenAsync(token, CancellationToken.None);

            if (refreshToken == null)
            {
                return null;
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadJwtToken(refreshToken.AccessToken);

            Claim userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid);
            Claim usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            Token regeneratedToken = await GenerateToken(issuer, audiences, userIdClaim.Value, usernameClaim.Value, 
                accessTokenExpirationInMinutes, refreshTokenExpirationInMinutes, customClaims, generateRefreshToken);

            await _tokenStore.DeleteAsync(token, CancellationToken.None);

            return regeneratedToken;
        }

        public async Task<Token> GenerateToken(string issuer, string[] audiences, string userId, string username,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes,
            IDictionary<string, object> customClaims = null, bool generateRefreshToken = false)
        {
            DateTimeOffset now = DateTime.UtcNow;
            DateTime refreshTokenExpirationDate = now.DateTime.AddMinutes(refreshTokenExpirationInMinutes);
            JwtPayload tokenPayload = SetPayload(issuer, audiences, userId, username, accessTokenExpirationInMinutes, now, customClaims);

            Token token = new Token();
            token.AccessToken = await WriteToken(tokenPayload);

            if (generateRefreshToken)
            {
                token.RefreshToken = Guid.NewGuid().ToString();
                
                RefreshToken refreshToken = new RefreshToken
                {
                    Token = token.RefreshToken,
                    UserId = long.Parse(userId),
                    ExpirationDate = refreshTokenExpirationDate,
                    AccessToken = token.AccessToken
                };

                IdentityResult identityResult = await _tokenStore.CreateRefreshTokenAsync(refreshToken, CancellationToken.None);
                if (!identityResult.Succeeded)
                {
                    return null;
                }
            }

            return token;
        }

        private async Task<string> WriteToken(JwtPayload payload)
        {
            RSAParameters rsaParameters = await _rsaManager.GetRsaPrivateKeyParameters();

            SigningCredentials credentials = new SigningCredentials(new RsaSecurityKey(rsaParameters), SecurityAlgorithms.RsaSha256);
            JwtHeader header = new JwtHeader(credentials);
            JwtSecurityToken securityToken = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            
            string token = handler.WriteToken(securityToken);
            return token;
        }

        private JwtPayload SetPayload(string issuer, string[] audiences, string userId, string username,
            double accessTokenExpirationInMinutes, DateTimeOffset dateTime, IDictionary<string, object> customClaims = null)
        {
            JwtPayload payload = new JwtPayload
            {
                { "jti", Guid.NewGuid().ToString() },
                { "sid", userId },
                { "sub", username },
                { "iss", issuer },
                { "aud", audiences },
                { "exp", DateTimeOffset.UtcNow.AddMinutes(accessTokenExpirationInMinutes).ToUnixTimeSeconds() },
                { "nbf", dateTime.ToUnixTimeSeconds() },
                { "iat", dateTime.ToUnixTimeSeconds() }
            };

            if (customClaims != null)
            {
                //TODO: implementar custom claims
            }

            return payload;
        }
    }
}