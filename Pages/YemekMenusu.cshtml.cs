using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KIVANC_WEB.Data;
using KIVANC_WEB.Models;

namespace KIVANC_WEB.Pages
{
    public class YemekMenusuModel : PageModel
    {
        // 1. Veritabaný baðlantýsý için bir deðiþken tanýmlýyoruz.
        private readonly ApplicationDbContext _context;

        // 2. Bu sayfa her oluþturulduðunda, veritabaný baðlantýsý otomatik olarak buraya gelir.
        public YemekMenusuModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // 3. HTML tarafýnda kullanacaðýmýz deðiþkenleri (özellikleri) tanýmlýyoruz.
        // Soru iþareti (?), bu deðiþkenlerin "boþ" (null) olabileceði anlamýna gelir.
        public YemekMenu? GununMenusu { get; set; }
        public List<YemekMenu> HaftalikMenu { get; set; } = new(); // Boþ liste olarak baþlatýyoruz.

        // 4. Sayfa ilk yüklendiðinde bu metot çalýþýr.
        public void OnGet()
        {
            // Bugünü ve 7 gün sonrasýnýn tarihini alýyoruz.
            var bugun = DateTime.Today;
            var haftaSonu = bugun.AddDays(7);

            // Veritabanýndan bugünün menüsünü buluyoruz.
            // FirstOrDefault: Eþleþen ilk kaydý getirir, bulamazsa hata vermez, boþ (null) döndürür.
            GununMenusu = _context.YemekMenuleri
                                  .FirstOrDefault(m => m.Tarih.Date == bugun.Date);

            // Veritabanýndan bugünden baþlayarak önümüzdeki 7 günün menülerini çekiyoruz.
            // Where: Þarta uyan tüm kayýtlarý getirir.
            // ToList: Gelen sonuçlarý bir listeye çevirir.
            HaftalikMenu = _context.YemekMenuleri
                                   .Where(m => m.Tarih.Date >= bugun.Date && m.Tarih.Date < haftaSonu.Date)
                                   .OrderBy(m => m.Tarih) // Tarihe göre sýralýyoruz.
                                   .ToList();
        }
    }
}
