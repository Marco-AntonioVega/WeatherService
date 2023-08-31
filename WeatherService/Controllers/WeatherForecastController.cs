using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("weather")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly Dictionary<int, string> Summaries = new Dictionary<int, string>(){{0,
            "Freezing" }, {4,"Bracing" }, {10,"Chilly" } , {16,"Cool" }, {21,"Mild" }, {27,"Warm" }, {32,"Hot" }, {38,"Sweltering" }, {43,"Scorching" }
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
         }
        
        //async function for API call
        [HttpGet(Name = "GetForecast")]
        public async Task<WeatherForecast> Get(String postal_code)
        {
            WeatherForecast forecast = await WeatherService.Implementations.WeatherForecastImplementation.GetWeatherForecast(postal_code);

            //todo: set Summary value on forecast response using Summaries data dictionary
            float summaryInt = forecast.temperature.celsius;

            //checks if temp is in Summaries; matches closest summary if not
            if(Summaries.ContainsKey((int)summaryInt)) {
                forecast.summary = Summaries[(int)summaryInt];
            }

            //finds closest summary int
            else {
                int lowerBound = 0;
                
                foreach(KeyValuePair<int, string> kvp in Summaries) {
                    if(kvp.Key < summaryInt) {
                        lowerBound = kvp.Key;
                    }
                    else if (kvp.Key > summaryInt) {
                        forecast.summary = (summaryInt - lowerBound <= kvp.Key - summaryInt) ? Summaries[lowerBound] : kvp.Value;
                        break;
                    }
                }
            }
            return forecast;
        }
    }
}