using KIVANC_WEB.Models;
using KIVANC_WEB.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KIVANC_WEB.Pages
{
    public class HavaDurumuModel : PageModel
    {
        private readonly WeatherService _weatherService;

        public HavaDurumuModel(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public WeatherForecast? Forecast { get; set; }

        public async Task OnGetAsync()
        {
            Forecast = await _weatherService.GetForecastAsync();
        }
    }
}