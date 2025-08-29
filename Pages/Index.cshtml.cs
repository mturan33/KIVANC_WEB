using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KIVANC_WEB.Services;

namespace KIVANC_WEB.Pages
{
    public class IndexModel : PageModel
    {
        public List<Personel> RastgelePersonel { get; set; } = new();
        public List<ServisHatti> RastgeleServisler { get; set; } = new();
        public List<IsEmri> SonUcIsEmirleri { get; set; } = new();

        // Mevcut constructor'� bununla de�i�tirin
        private readonly ApplicationDbContext _context;
        private readonly WeatherService _weatherService; // Yeni

        public IndexModel(ApplicationDbContext context, WeatherService weatherService) // Yeni
        {
            _context = context;
            _weatherService = weatherService; // Yeni
        }

        // --- HTML taraf�nda kullanaca��m�z �zellikler ---

        public WeatherForecast? Forecast { get; set; } // Yeni
        public YemekMenu? GununMenusu { get; set; }
        public List<IsgBildirim> SonUcIsgBildirimleri { get; set; } = new();
        public List<Duyuru> SonUcDuyurular { get; set; } = new(); // <-- YEN�: Son 3 duyuruyu tutmak i�in liste.

        public async Task OnGetAsync()
        {
            // --- G�N�N MEN�S�N� �EKME ---
            var bugun = DateTime.Today;
            GununMenusu = await _context.YemekMenuleri
                                        .FirstOrDefaultAsync(m => m.Tarih.Date == bugun.Date);

            // --- SON 3 ��Z�LMEM�� �SG B�LD�R�M�N� �EKME ---
            SonUcIsgBildirimleri = await _context.IsgBildirimleri
                                                 .Where(b => !b.CozulduMu)
                                                 .OrderByDescending(b => b.BildirimTarihi)
                                                 .Take(3)
                                                 .ToListAsync();

            // --- EN SON 3 DUYURUYU �EKME --- // <-- YEN� B�L�M
            SonUcDuyurular = await _context.Duyurular
                                           .OrderByDescending(d => d.YayinTarihi)
                                           .Take(3)
                                           .ToListAsync();

            SonUcIsEmirleri = await _context.IsEmirleri
                                .Where(i => !i.TamamlandiMi)
                                .OrderByDescending(i => i.OlusturmaTarihi)
                                .Take(3)
                                .ToListAsync();

            // --- RASTGELE 10 SERV�S HATTINI �EKME ---
            RastgeleServisler = await _context.ServisHatlar�
                                              .OrderBy(s => EF.Functions.Random()) // <-- DO�RU Y�NTEM
                                              .Take(10)
                                              .ToListAsync();

            // --- HAVA DURUMU VER�LER�N� �EKME ---
            Forecast = await _weatherService.GetForecastAsync();

            // --- RASTGELE 5 PERSONEL� �EKME ---
            RastgelePersonel = await _context.PersonelRehberi
                                             .OrderBy(p => EF.Functions.Random())
                                             .Take(5)
                                             .ToListAsync();

        }
    }
}