using Loja.Core.API.Controllers;
using Loja.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Cadastros.API.Controllers.v1
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TesteController : APIControllerBase
    {
        public TesteController(INotificator notifications)
            : base(notifications)
        {
        }

        [HttpGet("teste")]
        public IActionResult Teste()
        {
            return Ok();
        }
    }
}