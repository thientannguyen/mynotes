using MyNotes.Application.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyNotes.API.Controllers
{
    public class ClientErrorFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidCommandOrQueryException)
            {
                context.Result = new StatusCodeResult(400);
            }
        }
    }
}
