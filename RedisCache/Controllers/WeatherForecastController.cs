using Microsoft.AspNetCore.Mvc;
using RedisCache.Services;

namespace RedisCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICacheService _cacheService;
        public bool IsFromCache { get; set; } = false;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICacheService _cacheService)
        {
            _logger = logger;
            this._cacheService = _cacheService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var cacheData = await _cacheService.GetAsync<IEnumerable<WeatherForecast>>("weatherForecast");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            await _cacheService.SetAsync<IEnumerable<WeatherForecast>>("weatherForecast", cacheData);
            return cacheData;

        }
    }
}
