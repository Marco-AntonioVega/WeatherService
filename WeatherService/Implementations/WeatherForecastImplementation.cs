using System.Text.Json;

namespace WeatherService.Implementations
{
    public class WeatherForecastImplementation
    {
        //async function for API call
        internal static async Task<WeatherForecast> GetWeatherForecast(String postal_code)
        {
            using HttpClient httpClient = new HttpClient();

            //todo: retreive current temperature from:
            //https://www.weatherbit.io/api/swaggerui/weather-api-v2#!/Current32Weather32Data/get_current_postal_code_postal_code
            //using API Key: 1824631bbfa74729aac7d2d2f1dfdd76

            String apiKey = "1824631bbfa74729aac7d2d2f1dfdd76";
            String url = $"https://api.weatherbit.io/v2.0/current?postal_code={postal_code}&key={apiKey}";

            try {
                //API call
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode) {

                    //takes in response as string
                    var weatherData = await response.Content.ReadAsStringAsync();

                    //check if weatherData is empty or null
                    if (string.IsNullOrEmpty(weatherData)) {
                        System.Diagnostics.Debug.WriteLine("Empty response");
                        return new WeatherForecast();
                    }

                    //serializes the "data" attribute to WeatherForecastData class
                    WeatherForecastData weatherDataArray = JsonSerializer.Deserialize<WeatherForecastData>(weatherData);

                    //check if weatherDataArray.data is empty or null
                    if (weatherDataArray == null || weatherDataArray.data == null || weatherDataArray.data.Count == 0) {
                        System.Diagnostics.Debug.WriteLine("No weather data");
                        return new WeatherForecast();
                    }

                    //serializes elements of the "data" attribute to WeatherForecastDataElements class
                    WeatherForecastDataElements weatherDataElements = JsonSerializer.Deserialize<WeatherForecastDataElements>(weatherDataArray.data[0].ToString());

                    //check if weatherDataElements is null
                    if (weatherDataElements == null) {
                        System.Diagnostics.Debug.WriteLine("No weather data");
                        return new WeatherForecast();
                    }

                    //assigns necessary values of API call to new WeatherForecast instance
                    WeatherForecast forecast = new WeatherForecast {
                        date = weatherDataElements.ob_time,
                        temperature = new TempStruct {
                            celsius = (float)Math.Round(weatherDataElements.temp, 1),
                            fahrenheit = (float)Math.Round(weatherDataElements.temp * 9 / 5 + 32, 1)
                        }
                    };

                    return forecast;
                }
                else {
                    System.Diagnostics.Debug.WriteLine("HTTP request failed");
                    return new WeatherForecast(); //HTTP request failed
                }
            }

            catch (HttpRequestException) {
                System.Diagnostics.Debug.WriteLine("HTTP request exception");
                return new WeatherForecast(); //Handle specific HTTP request exception
            }

            catch (JsonException) {
                System.Diagnostics.Debug.WriteLine("Json deserialization exception");
                return new WeatherForecast(); //Handle Json deserialization exception
            }

            catch (Exception) {
                System.Diagnostics.Debug.WriteLine("Exception");
                return new WeatherForecast(); //Handles other exceptions
            }
        }
    }
}
