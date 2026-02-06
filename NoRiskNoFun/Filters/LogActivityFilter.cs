using Microsoft.AspNetCore.Mvc.Filters;

namespace NoRiskNoFun.Filters
{
    public class LogActivityFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
           _logger.LogInformation("Action {ActionName} is executing at {Time}", context.ActionDescriptor.DisplayName, DateTime.UtcNow);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Action {ActionName} executed at {Time}", context.ActionDescriptor.DisplayName, DateTime.UtcNow);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Action {ActionName} is executing at {Time}", context.ActionDescriptor.DisplayName, DateTime.UtcNow);
            await next();
            _logger.LogInformation("Action {ActionName} executed at {Time}", context.ActionDescriptor.DisplayName, DateTime.UtcNow);
        }
    }
}
