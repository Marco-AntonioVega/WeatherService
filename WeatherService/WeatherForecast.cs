namespace WeatherService
{
  //WeatherForecast structure to exactly match expected output format
    public struct TempStruct {
        public float celsius { get; set; }
        public float fahrenheit { get; set; }
    }
    public class WeatherForecast {
        public String? date { get; set; }

        public TempStruct temperature { get; set; }
        public String? summary { get; set; }
    }
}