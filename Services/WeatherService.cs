using KIVANC_WEB.Models;
using System.Diagnostics; // <-- BU SATIRI EKLEYİN
using System.Text.Json;

namespace KIVANC_WEB.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast?> GetForecastAsync()
        {
            var apiUrl = "https://api.open-meteo.com/v1/forecast?latitude=36.99&longitude=35.17&hourly=temperature_2m&daily=weathercode,temperature_2m_max,temperature_2m_min&current=temperature_2m,weathercode&timezone=auto&forecast_days=7";

            try
            {
                Debug.WriteLine("--- Hava Durumu API İsteği Başlatılıyor ---");
                var response = await _httpClient.GetAsync(apiUrl);

                // API'den gelen HTTP durum kodunu yazdıralım (200 OK olmalı)
                Debug.WriteLine($"API Yanıt Kodu: {response.StatusCode}");

                var jsonResponse = await response.Content.ReadAsStringAsync();

                // API'den gelen ham JSON verisini Çıktı penceresine yazdıralım
                Debug.WriteLine("--- API'den Gelen Ham JSON Yanıtı ---");
                Debug.WriteLine(jsonResponse);
                Debug.WriteLine("--------------------------------------");

                // Hata varsa burada exception fırlatılacak
                response.EnsureSuccessStatusCode();

                var forecast = JsonSerializer.Deserialize<WeatherForecast>(jsonResponse);

                if (forecast == null)
                {
                    Debug.WriteLine("HATA: JSON Deserialization işlemi null bir nesne döndürdü!");
                }
                else
                {
                    Debug.WriteLine("JSON Deserialization başarılı.");
                }

                return forecast;
            }
            catch (Exception e)
            {
                // Herhangi bir hata olursa (ağ hatası, JSON hatası vb.) buraya düşer
                Debug.WriteLine($"API İSTEĞİNDE VEYA İŞLEMEDE BİR HATA OLUŞTU: {e.Message}");
                return null;
            }
        }
    }
}