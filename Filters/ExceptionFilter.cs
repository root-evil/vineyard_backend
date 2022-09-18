using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Net;

using vineyard_backend.Models;

namespace vineyard_backend.Filters
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        private const string HeaderCFRAY = "cF-RAY";
        private const string HeaderXRequestID = "x-Request-ID";

        private readonly ILogger<ExceptionFilter> logger;
        private const string DetectedProblem = "We detected a problem";

        public ExceptionFilter(
            ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public int Order { get; set; } = int.MaxValue - 10;

        private static string GetValueFromHeades(string key, FilterContext context)
        {
            return context?.HttpContext?.Request?.Headers is not null && context.HttpContext.Request.Headers.TryGetValue(key, out var value)
                ? value.ToString()
                : string.Empty;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Executing, TraceIdentifier: {TraceIdentifier}, RemoteIpAddress: {RemoteIpAddress}, x-Request-ID: {xRequestID}, cF-RAY: {cFRAY}, Request: [{Method}] {Path}",
                context?.HttpContext?.TraceIdentifier,
                context?.HttpContext?.Connection?.RemoteIpAddress,
                GetValueFromHeades(HeaderXRequestID, context),
                GetValueFromHeades(HeaderCFRAY, context),
                context?.HttpContext?.Request?.Method,
                context?.HttpContext?.Request?.Path);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context?.Exception is Exception exception)
            {
                logger.LogError(exception, "TraceIdentifier: {TraceIdentifier}, RemoteIpAddress: {RemoteIpAddress}, x-Request-ID: {xRequestID}, cF-RAY: {cFRAY}, Request: [{Method}] {Path}",
                    context?.HttpContext?.TraceIdentifier,
                    context?.HttpContext?.Connection?.RemoteIpAddress,
                    GetValueFromHeades(HeaderXRequestID, context),
                    GetValueFromHeades(HeaderCFRAY, context),
                    context?.HttpContext?.Request?.Method,
                    context?.HttpContext?.Request?.Path);

                BaseResponseError resultResponseError = new BaseResponseError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Произошла ошибка",
                    Description = exception.Message.ToString(),
                    TraceId = context?.HttpContext?.TraceIdentifier,
                };

                context.Result ??= new BadRequestObjectResult(resultResponseError);
                context.ExceptionHandled = true;
            }
        }
    }
}
