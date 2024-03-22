using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Reddit.Middlewares
{
    public class ExceptionHandler2 : IExceptionHandler
    {

            private readonly ILogger<ExceptionHandler2> logger;
            public ExceptionHandler2(ILogger<ExceptionHandler2> logger)
            {
                this.logger = logger;
            }
                        public async ValueTask<bool> TryHandleAsync(
                         HttpContext httpContext,
                         Exception exception,
                         CancellationToken cancellationToken)
                {

            logger.LogError(exception, "An error occurred while processing your request.");
            logger.LogError("An error occurred while processing your request: {ErrorMessage}", exception.Message);


            var problemDetails = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type = exception.GetType().Name,
                        Title = "An unhandled error occurred",
                        Detail = exception.Message
                    };

                    await httpContext
                        .Response
                        .WriteAsJsonAsync(problemDetails, cancellationToken);

                    return true;
            // you want to write many handlers
                }
    }
}
