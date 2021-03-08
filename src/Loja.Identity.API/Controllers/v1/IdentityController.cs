using System;
using System.Threading.Tasks;
using Loja.Core.API.Controllers;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.OAuth;
using Loja.Core.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Loja.Identity.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class IdentityController : APIControllerBase
    {
        private readonly IOAuthService _authenticationService;
        private readonly IConfiguration _configuration;

        public IdentityController(IOAuthService authenticationService,
            IConfiguration configuration,
            INotificator notifications)
            : base(notifications)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                TokenConfiguration tokenConfiguration = _configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();

                Token token = await _authenticationService.GetTokenAsync(tokenRequest, tokenConfiguration.Issuer, tokenConfiguration.Audiences, 
                    tokenConfiguration.ExpirationInMinutesAccessToken, tokenConfiguration.ExpirationInMinutesRefreshToken);

                return APIResult(result: token);
            }
            catch (Exception ex)
            {
                return APIResult(ex);
            }
        }
    }
}