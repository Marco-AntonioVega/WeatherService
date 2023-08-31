namespace WeatherService {

  //serializes datetime and temperature from the "data" array attribute
  public class WeatherForecastDataElements {
      public String? ob_time { get; set; }
      public float temp { get; set; }
  }
}
