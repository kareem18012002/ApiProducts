namespace NoRiskNoFun.Middleware
{
    public class ProfilingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfilingMiddleware> _logger;

        public ProfilingMiddleware(RequestDelegate next , ILogger <ProfilingMiddleware> logger)
        {
          _next = next;
            _logger = logger;   
        }
        public async Task Invoke(HttpContext context) // Middleware Invoke method.
        { 
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await _next(context); // Call the next middleware in the pipeline
            stopwatch.Stop();// Stop the stopwatch after the next middleware has completed.
            var elapsedMs = stopwatch.ElapsedMilliseconds;// Get the elapsed time in milliseconds.
            _logger.LogInformation("Request [{Method}] {Path} executed in took {ElapsedMilliseconds} ms", context.Request.Method, context.Request.Path, elapsedMs); // Log the request method, path, and elapsed time.
        }
    }
}
