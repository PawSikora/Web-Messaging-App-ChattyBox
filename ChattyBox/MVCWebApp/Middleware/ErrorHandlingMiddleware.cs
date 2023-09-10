using BLL.Exceptions;

namespace MVCWebApp.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) => _logger = logger;


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }

            catch (NotFoundException ex)
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            }

            catch (NotUniqueElementException ex)
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }

            catch (IllegalOperationException ex)
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }

            catch (EmailAlreadyUsedException ex)
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }

            catch (LoginFailedException ex)
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Wystąpił błąd: {ErrorMessage}", ex.Message);
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Coś poszło nie tak");
            }
        }
    }
}
