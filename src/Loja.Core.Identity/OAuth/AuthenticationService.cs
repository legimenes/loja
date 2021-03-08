using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loja.Core.Identity.Managers;
using Loja.Core.Identity.Models;
using Loja.Core.Notifications;
using Microsoft.AspNetCore.Identity;

namespace Loja.Core.Identity.OAuth
{
    public class OAuthService : IOAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly AppUserManager<User> _userManager;
        private readonly ITokenManager _tokenManager;
        private readonly INotificator _notifications;

        public OAuthService(SignInManager<User> signInManager,
            AppUserManager<User> userManager,
            ITokenManager tokenManager,
            INotificator notifications)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenManager = tokenManager;
            _notifications = notifications;
        }

        public async Task<Token> GetTokenAsync(TokenRequest tokenRequest, string issuer, string[] audiences,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes)
        {
            try
            {
                switch (tokenRequest.grant_type)
                {
                    case "password":
                        return await GenerateToken(tokenRequest, issuer, audiences, accessTokenExpirationInMinutes, refreshTokenExpirationInMinutes);
                     case "refresh_token":
                         return await GenerateRefreshToken(tokenRequest.refresh_token, issuer, audiences, accessTokenExpirationInMinutes,
                            refreshTokenExpirationInMinutes);
                    default:
                        _notifications.Notify(new Notification(NotificationType.Fail, "Invalid grant type."));
                        return null;
                }
            }
            catch (Exception exception)
            {
                //TODO log
                _notifications.Notify(new Notification(exception));
                return null;
            }
        }

        private async Task<Token> GenerateRefreshToken(string token, string issuer, string[] audiences,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes,
            IDictionary<string, object> customClaims = null)
        {
            try
            {
                Token regeneratedToken = await _tokenManager.GenerateToken(token, issuer, audiences, accessTokenExpirationInMinutes,
                    refreshTokenExpirationInMinutes, customClaims, true);

                if (regeneratedToken == null)
                {
                    _notifications.Notify(new Notification(NotificationType.Fail, "Invalid refresh token."));
                }

                return regeneratedToken;
            }
            catch (Exception exception)
            {
                //TODO log
                _notifications.Notify(new Notification(exception));
                return null;
            }
        }

        private async Task<Token> GenerateToken(TokenRequest tokenRequest, string issuer, string[] audiences,
            double accessTokenExpirationInMinutes, double refreshTokenExpirationInMinutes,
            IDictionary<string, object> customClaims = null)
        {
            try
            {
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(tokenRequest.username, tokenRequest.password, false, false);
                if (!signInResult.Succeeded)
                {
                    _notifications.Notify(new Notification(NotificationType.Fail, "Sign-in attempt failed."));
                    return null;
                }

                User user = await _userManager.FindByNameAsync(tokenRequest.username);

                Token token = await _tokenManager.GenerateToken(issuer, audiences, user.Id.ToString(), tokenRequest.username, 
                    accessTokenExpirationInMinutes, refreshTokenExpirationInMinutes, customClaims, true);

                return token;
            }
            catch (Exception exception)
            {
                //TODO log
                _notifications.Notify(new Notification(exception));
                return null;
            }
        }
    }
}