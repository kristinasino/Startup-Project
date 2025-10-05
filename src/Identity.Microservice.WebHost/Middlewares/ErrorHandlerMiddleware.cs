using System.Net;
using System.Text.Json;
using Shared.Common.Exceptions;

namespace Identity.Microservice.WebHost.Middlewares
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var issueId = Guid.NewGuid();
                var response = context.Response;
                response.ContentType = "application/json";

                switch(error)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        break;
                    case BadRequestException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogError(error, "ERROR_LOG: " + error.Message + " {IssueId}", issueId);
                        break;
                }

                var warningExceptionsResponse = new { message = error?.Message, status = response.StatusCode };
                var unproccesableExceptionsResponse = new { message = GetErrors(error), status = response.StatusCode };
                var internalExceptionsResponse =
                    new { message = error?.Message, status = response.StatusCode, issueId };
                var result = JsonSerializer.Serialize(response.StatusCode == (int)HttpStatusCode.InternalServerError 
                    ? (object)internalExceptionsResponse 
                    : response.StatusCode == (int)HttpStatusCode.UnprocessableEntity ? unproccesableExceptionsResponse
                    : warningExceptionsResponse);
                await response.WriteAsync(result);
            }
        }
        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
        {
            IReadOnlyDictionary<string, string[]>? errors = null;
            if (exception is ValidationException validationException)
            {
                errors = validationException.ErrorsDictionary;
            }
            return errors;
        }
    }
}
