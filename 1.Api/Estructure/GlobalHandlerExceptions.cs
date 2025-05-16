using System.Net;
using Almacen._2.Common.Models.System;

namespace Almacen._1.Api.Estructure
{
    public class GlobalHandlerExceptions
    {
        private readonly RequestDelegate _nextDelegate;
        public GlobalHandlerExceptions(RequestDelegate nextDelegate)
        {
            this._nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this._nextDelegate(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }
        private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = exception switch
            {
                AppBadRequestException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException or AppNotFoundException => (int)HttpStatusCode.NotFound,
                HttpRequestException => (int)HttpStatusCode.ServiceUnavailable,
                InvalidOperationException => (int)HttpStatusCode.InternalServerError,
                ConflictException => (int)HttpStatusCode.Conflict,
                ServicesException servicesException => servicesException.StatusCode,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorResponse = new ErrorResponse();
            var message = exception.Message;

            errorResponse.Errors.Add(message);
            return httpContext.Response.WriteAsync(errorResponse.ToString());
        }
    }
}
