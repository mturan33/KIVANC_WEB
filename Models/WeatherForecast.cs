using System.Text.Json.Serialization;

namespace KIVANC_WEB.Models
{
    public class WeatherForecast
    {
        [JsonPropertyName("current")]
        public CurrentWeather? Current { get; set; }

        [JsonPropertyName("hourly")]
        public HourlyData? Hourly { get; set; }

        [JsonPropertyName("daily")]
        public DailyData? Daily { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }

        [JsonPropertyName("weathercode")]
        public int WeatherCode { get; set; }
    }

    public class HourlyData
    {
        [JsonPropertyName("time")]
        public List<string>? Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public List<double>? Temperature2m { get; set; }
    }

    public class DailyData
    {
        [JsonPropertyName("time")]
        public List<string>? Time { get; set; }

        [JsonPropertyName("weathercode")]
        public List<int>? WeatherCode { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double>? Temperature2mMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double>? Temperature2mMin { get; set; }
    }
}