namespace NoRiskNoFun.services.Test
{
    public class WeatherForecastServices
    {
        private readonly ILogger<WeatherForecastServices> _logger;
        public WeatherForecastServices(ILogger<WeatherForecastServices> logger)
        {
            _logger = logger;
        }
        private static readonly string[] Summaries =
       [
           "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
       ];
        public IEnumerable<WeatherForecast> GetForecasts()
        {
            _logger.LogInformation("Generating weather forecasts.");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}