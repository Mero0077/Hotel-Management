using Hotel_Management.Exceptions;
using Hotel_Management.Models.Enums;
using Hotel_Management.DTOs.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hotel_Management.MiddleWares
{
    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalErrorHandlerMiddleware(
            ILogger<GlobalErrorHandlerMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var traceId = context.TraceIdentifier;

            try
            {
                _logger.LogInformation($"Request started. TraceId: {traceId}, Path: {context.Request.Path}, Method: {context.Request.Method}");

                await next(context);

                _logger.LogInformation($"Request completed successfully. TraceId: {traceId}, StatusCode: {context.Response.StatusCode}");
            }
            catch (NotFoundException notFoundEx)
            {
                await HandleNotFoundExceptionAsync(context, notFoundEx, traceId);
            }
            catch (ValidationException validationEx)
            {
                await HandleValidationExceptionAsync(context, validationEx, traceId);
            }
            catch (BaseApplicationException appEx)
            {
                await HandleApplicationExceptionAsync(context, appEx, traceId);
            }
            catch (UnauthorizedAccessException unauthorizedEx)
            {
                await HandleUnauthorizedExceptionAsync(context, unauthorizedEx, traceId);
            }
            catch (Exception ex)
            {
                await HandleUnexpectedExceptionAsync(context, ex, traceId);
            }
        }
        private async Task HandleNotFoundExceptionAsync(HttpContext context, BaseApplicationException exception, string traceId)
        {
            var logLevel = exception is BusinessLogicException ? LogLevel.Warning : LogLevel.Error;

            _logger.Log(
                logLevel,
                exception,
                $"Application Exception. TraceId: {traceId}, Message: {exception.Message}, ErrorCode: {exception.ErrorCode}, RequestPath: {context.Request.Path}");

            var response = new ErrorFailDTO<object>(exception.ErrorCode, exception.Message)
            {
                TraceId = _environment.IsDevelopment() ? traceId : null
            };

            await WriteErrorResponseAsync(context, response, exception.HttpStatusCode, traceId);
        }
        private async Task HandleValidationExceptionAsync(HttpContext context, BaseApplicationException exception, string traceId)
        {
            var logLevel = exception is BusinessLogicException ? LogLevel.Warning : LogLevel.Error;

            _logger.Log(
                logLevel,
                exception,
                $"Application Exception. TraceId: {traceId}, Message: {exception.Message}, ErrorCode: {exception.ErrorCode}, RequestPath: {context.Request.Path}");

            var response = new ErrorFailDTO<object>(exception.ErrorCode, exception.Message)
            {
                TraceId = _environment.IsDevelopment() ? traceId : null
            };

            await WriteErrorResponseAsync(context, response, exception.HttpStatusCode, traceId);
        }
        private async Task HandleApplicationExceptionAsync(HttpContext context, BaseApplicationException exception, string traceId)
        {
            var logLevel = exception is BusinessLogicException ? LogLevel.Warning : LogLevel.Error;

            _logger.Log(
                logLevel,
                exception,
                $"Application Exception. TraceId: {traceId}, Message: {exception.Message}, ErrorCode: {exception.ErrorCode}, RequestPath: {context.Request.Path}");

            var response = new ErrorFailDTO<object>(exception.ErrorCode, exception.Message)
            {
                TraceId = _environment.IsDevelopment() ? traceId : null
            };

            await WriteErrorResponseAsync(context, response, exception.HttpStatusCode, traceId);
        }

        private async Task HandleUnauthorizedExceptionAsync(HttpContext context, UnauthorizedAccessException exception, string traceId)
        {
            _logger.LogWarning(
                exception,
                "Unauthorized Access Exception. TraceId: {TraceId}, Message: {Message}, RequestPath: {RequestPath}",
                traceId,
                exception.Message,
                context.Request.Path);

            var response = new ErrorFailDTO<object>(ErrorCode.UnauthorizedAccess, "Access Denied")
            {
                TraceId = _environment.IsDevelopment() ? traceId : null
            };

            await WriteErrorResponseAsync(context, response, StatusCodes.Status403Forbidden, traceId);
        }

        private async Task HandleUnexpectedExceptionAsync(HttpContext context, Exception exception, string traceId)
        {
            _logger.LogError(
                exception,
                "Unexpected error occurred. TraceId: {TraceId}, RequestPath: {RequestPath}",
                traceId,
                context.Request.Path);

            var response = new ErrorFailDTO<object>(
                ErrorCode.InternalServerError,
                _environment.IsDevelopment()
                    ? $"An unexpected error occurred. TraceId: {traceId}"
                    : "An unexpected error occurred.")
            {
                TraceId = _environment.IsDevelopment() ? traceId : null
            };

            await WriteErrorResponseAsync(context, response, StatusCodes.Status500InternalServerError, traceId);
        }

        private async Task WriteErrorResponseAsync<T>(HttpContext context, ResponseDTO<T> response, int statusCode, string traceId)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("X-Trace-Id", traceId);

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await context.Response.WriteAsJsonAsync(response, jsonOptions);
        }
    }
}
