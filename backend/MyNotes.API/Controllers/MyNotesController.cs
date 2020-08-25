using System;
using MyNotes.Application.SharedKernel;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyNotes.API.Controllers
{
    [ClientErrorFilter]
    public abstract class MyNotesController : ControllerBase
    {
        protected ObjectResult FromResult<T>(Result<T> result)
        {
            return result.Match(
                value => Ok(value),
                ex => ex is InvalidCommandOrQueryException
                    ? BadRequestWithoutResult()
                    : ServerError(ex));
        }

        protected ObjectResult ServerError(Exception ex) => StatusCode(StatusCodes.Status500InternalServerError, ex);

        protected ObjectResult BadRequestWithoutResult() => StatusCode(StatusCodes.Status400BadRequest, null);
    }
}
