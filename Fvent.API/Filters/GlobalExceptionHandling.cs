using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Fvent.BO.Exceptions;

namespace Fvent.API.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// Handle global exception
    /// </summary>
    /// <param name="context"></param>
    public void OnException(ExceptionContext context)
    {
        // Phan loai exception
        var statusCode = context.Exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        // Tra ket qua
        context.Result = new ObjectResult(new
        {
            error = context.Exception.Message,
            innerException = context.Exception.InnerException?.Message,
#if DEBUG
            stackTrace = context.Exception.StackTrace,
#endif
        })
        {
            StatusCode = statusCode
        };
    }
}
