using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Bu using ifadesinin olduðundan emin olun.

namespace KIVANC_WEB.Pages
{
    public class YemekMenusuModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public YemekMenusuModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public YemekMenu? GununMenusu { get; set; }
        public List<YemekMenu> HaftalikMenu { get; set; } = new();

        // Sayfa ilk yüklendiðinde bu metot çalýþýr.
        public async Task OnGetAsync()
        {
            // --- YENÝ ADIM: Yeni Ayýn Menüsünün Gerekli Olup Olmadýðýný Kontrol Et ve Oluþtur ---
            await EnsureNextMonthMenuExistsAsync();

            // --- Mevcut Kod: Verileri Çek ve Göster ---
            var bugun = DateTime.Today;
            var haftaSonu = bugun.AddDays(7);

            GununMenusu = await _context.YemekMenuleri
                                        .FirstOrDefaultAsync(m => m.Tarih.Date == bugun.Date);

            HaftalikMenu = await _context.YemekMenuleri
                                         .Where(m => m.Tarih.Date >= bugun.Date && m.Tarih.Date < haftaSonu.Date)
                                         .OrderBy(m => m.Tarih)
                                         .ToListAsync();
        }

        /// <summary>
        /// Veritabanýný kontrol eder ve gerekirse bir sonraki ayýn menüsünü otomatik olarak oluþturur.
        /// </summary>
        private async Task EnsureNextMonthMenuExistsAsync()
        {
            // Adým 1: Veritabanýndaki en son menü kaydýný bul.
            var sonMenuKaydi = await _context.YemekMenuleri.OrderByDescending(m => m.Tarih).FirstOrDefaultAsync();
            var bugun = DateTime.Today;

            // Eðer veritabaný tamamen boþsa veya en son kayýt zaten bu ay veya gelecek bir aya aitse, hiçbir þey yapma.
            if (sonMenuKaydi == null || (sonMenuKaydi.Tarih.Year >= bugun.Year && sonMenuKaydi.Tarih.Month >= bugun.Month))
            {
                return; // Menü güncel, iþlem yapmaya gerek yok.
            }

            // Adým 2: Bir önceki ayý þablon olarak kullanmak için o ayýn menüsünü çek (Pazar günleri hariç).
            var sonAyinBasi = new DateTime(sonMenuKaydi.Tarih.Year, sonMenuKaydi.Tarih.Month, 1);
            var sonAyinSonu = sonAyinBasi.AddMonths(1).AddDays(-1);

            var menuSablonu = await _context.YemekMenuleri
                                            .Where(m => m.Tarih >= sonAyinBasi && m.Tarih <= sonAyinSonu && m.AnaYemek != "Yemekhane Kapalý")
                                            .OrderBy(m => m.Tarih)
                                            .ToListAsync();

            // Eðer þablon oluþturulamadýysa (veri yoksa) iþlemi durdur.
            if (!menuSablonu.Any())
            {
                return;
            }

            // Adým 3: Ýçinde bulunduðumuz ay için yeni menü listesi oluþtur.
            var yeniMenuKayitlari = new List<YemekMenu>();
            var ayinIlkGunu = new DateTime(bugun.Year, bugun.Month, 1);
            var aydakiGunSayisi = DateTime.DaysInMonth(bugun.Year, bugun.Month);
            int sablonIndex = 0; // Þablonda hangi yemeði kullanacaðýmýzý takip etmek için sayaç.

            for (int i = 0; i < aydakiGunSayisi; i++)
            {
                var oGun = ayinIlkGunu.AddDays(i);
                var yeniMenu = new YemekMenu { Tarih = oGun };

                // Eðer gün Pazar ise, "Yemekhane Kapalý" olarak ayarla.
                if (oGun.DayOfWeek == DayOfWeek.Sunday)
                {
                    yeniMenu.Corba = "---";
                    yeniMenu.AnaYemek = "Yemekhane Kapalý";
                    yeniMenu.Tatli = "---";
                }
                else // Gün Pazar deðilse, þablondan sýradaki yemeði ata.
                {
                    var sablondakiYemek = menuSablonu[sablonIndex];
                    yeniMenu.Corba = sablondakiYemek.Corba;
                    yeniMenu.AnaYemek = sablondakiYemek.AnaYemek;
                    yeniMenu.Tatli = sablondakiYemek.Tatli;

                    // Döngüsel olarak bir sonraki þablon yemeðine geç.
                    sablonIndex = (sablonIndex + 1) % menuSablonu.Count;
                }
                yeniMenuKayitlari.Add(yeniMenu);
            }

            // Adým 4: Oluþturulan yeni ayýn menüsünü veritabanýna toplu olarak ekle.
            await _context.YemekMenuleri.AddRangeAsync(yeniMenuKayitlari);
            await _context.SaveChangesAsync();
        }
    }
}