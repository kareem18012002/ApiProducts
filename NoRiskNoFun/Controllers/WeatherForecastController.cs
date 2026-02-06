using Microsoft.AspNetCore.Mvc;
using NoRiskNoFun.services.Test;

namespace NoRiskNoFun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastServices _services;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , WeatherForecastServices services)
        {
            _logger = logger;
            _services = services;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
         
            return _services.GetForecasts();

        }
    }
}
