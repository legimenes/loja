using System;
using System.Threading.Tasks;
using Loja.Cadastros.Domain.Localidades.Entities;
using Loja.Core.API.Controllers;
using Loja.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Cadastros.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LocalidadesController : APIControllerBase
    {
        public LocalidadesController(INotificator notifications)
            : base(notifications)
        {
        }

        [HttpGet("cidade")]
        public async Task<IActionResult> ObterCidade()
        {
            //try
            //{
                int i = 0;
                int j = 2 / i;

                _notifications.Notify(new Notification(NotificationType.Fail, "Erro 1"));
                _notifications.Notify(new Notification(NotificationType.Fail, "Erro 2"));
                _notifications.Notify(new Notification(NotificationType.Fail, "Erro 3"));

                Cidade cidade = new Cidade();
                cidade.Nome = "Sao Paulo";
                cidade.IdUF = 25;
                //Cidade.UF = "SP";
                cidade.CodigoIBGE = "35";

                return APIResult(result: cidade);
            //}
            //catch (Exception exception)
            //{
            //    return APIResult(exception);
            //}
        }
    }
}