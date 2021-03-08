using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Core.API.Behaviors
{
    public class InvalidModelStateResponse
    {
        public static IActionResult SetModelStateResponse(ActionContext context)
        {
            List<string> errors = context.ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            BadRequestObjectResult result = new BadRequestObjectResult(errors);

            return result;
        }
    }
}