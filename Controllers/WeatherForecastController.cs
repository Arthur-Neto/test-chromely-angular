using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Test.Chromely.Controllers
{
    [ControllerProperty(Name = nameof(WeatherForecastController))]
    public class WeatherForecastController : ChromelyController
    {
        private readonly IChromelyConfiguration _config;
        private readonly IChromelySerializerUtil _serializerUtil;
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IChromelyConfiguration config,
            IChromelySerializerUtil serializerUtil)
        {
            _logger = logger;
            _config = config;
            _serializerUtil = serializerUtil;

            RegisterRequest("/WeatherForecastController/Get", Get);
        }

        private IChromelyResponse Get(IChromelyRequest request)
        {
            var rng = new Random();
            var gen = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return new ChromelyResponse(request.Id)
            {
                Data = gen
            };
        }
    }
}
