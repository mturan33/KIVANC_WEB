using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KIVANC_WEB.Data;
using KIVANC_WEB.Models;

namespace KIVANC_WEB.Pages
{
    public class YemekMenusuModel : PageModel
    {
        // 1. Veritaban� ba�lant�s� i�in bir de�i�ken tan�ml�yoruz.
        private readonly ApplicationDbContext _context;

        // 2. Bu sayfa her olu�turuldu�unda, veritaban� ba�lant�s� otomatik olarak buraya gelir.
        public YemekMenusuModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // 3. HTML taraf�nda kullanaca��m�z de�i�kenleri (�zellikleri) tan�ml�yoruz.
        // Soru i�areti (?), bu de�i�kenlerin "bo�" (null) olabilece�i anlam�na gelir.
        public YemekMenu? GununMenusu { get; set; }
        public List<YemekMenu> HaftalikMenu { get; set; } = new(); // Bo� liste olarak ba�lat�yoruz.

        // 4. Sayfa ilk y�klendi�inde bu metot �al���r.
        public void OnGet()
        {
            // Bug�n� ve 7 g�n sonras�n�n tarihini al�yoruz.
            var bugun = DateTime.Today;
            var haftaSonu = bugun.AddDays(7);

            // Veritaban�ndan bug�n�n men�s�n� buluyoruz.
            // FirstOrDefault: E�le�en ilk kayd� getirir, bulamazsa hata vermez, bo� (null) d�nd�r�r.
            GununMenusu = _context.YemekMenuleri
                                  .FirstOrDefault(m => m.Tarih.Date == bugun.Date);

            // Veritaban�ndan bug�nden ba�layarak �n�m�zdeki 7 g�n�n men�lerini �ekiyoruz.
            // Where: �arta uyan t�m kay�tlar� getirir.
            // ToList: Gelen sonu�lar� bir listeye �evirir.
            HaftalikMenu = _context.YemekMenuleri
                                   .Where(m => m.Tarih.Date >= bugun.Date && m.Tarih.Date < haftaSonu.Date)
                                   .OrderBy(m => m.Tarih) // Tarihe g�re s�ral�yoruz.
                                   .ToList();
        }
    }
}
