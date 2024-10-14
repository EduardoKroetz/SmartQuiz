using Microsoft.AspNetCore.Diagnostics;
using QuizDev.Core.DTOs.Responses;

namespace QuizDev.API.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var message = exception.Message;
        var status = 500;

        switch (exception)
        {
            case InvalidOperationException:
            case ArgumentException:
                status = 400;
                break;
            case UnauthorizedAccessException:
                status = 403;
                break;
            default:
                status = 500;
                break;  
        }

        httpContext.Response.StatusCode = status;
        var content = new ResultDto([message]);
        await httpContext.Response.WriteAsJsonAsync(content, cancellationToken);
        return true;
    }
}
