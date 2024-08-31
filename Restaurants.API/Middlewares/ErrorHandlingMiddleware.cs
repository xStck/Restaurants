using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException nfe)
        {
            logger.LogWarning(nfe.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(nfe.Message);
        }
        catch (ForbidException fe)
        {
            logger.LogWarning(fe.Message);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access forbidden");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong.");
        }
    }
}