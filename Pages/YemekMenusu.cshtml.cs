using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Bu using ifadesinin oldu�undan emin olun.

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

        // Sayfa ilk y�klendi�inde bu metot �al���r.
        public async Task OnGetAsync()
        {
            // --- YEN� ADIM: Yeni Ay�n Men�s�n�n Gerekli Olup Olmad���n� Kontrol Et ve Olu�tur ---
            await EnsureNextMonthMenuExistsAsync();

            // --- Mevcut Kod: Verileri �ek ve G�ster ---
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
        /// Veritaban�n� kontrol eder ve gerekirse bir sonraki ay�n men�s�n� otomatik olarak olu�turur.
        /// </summary>
        private async Task EnsureNextMonthMenuExistsAsync()
        {
            // Ad�m 1: Veritaban�ndaki en son men� kayd�n� bul.
            var sonMenuKaydi = await _context.YemekMenuleri.OrderByDescending(m => m.Tarih).FirstOrDefaultAsync();
            var bugun = DateTime.Today;

            // E�er veritaban� tamamen bo�sa veya en son kay�t zaten bu ay veya gelecek bir aya aitse, hi�bir �ey yapma.
            if (sonMenuKaydi == null || (sonMenuKaydi.Tarih.Year >= bugun.Year && sonMenuKaydi.Tarih.Month >= bugun.Month))
            {
                return; // Men� g�ncel, i�lem yapmaya gerek yok.
            }

            // Ad�m 2: Bir �nceki ay� �ablon olarak kullanmak i�in o ay�n men�s�n� �ek (Pazar g�nleri hari�).
            var sonAyinBasi = new DateTime(sonMenuKaydi.Tarih.Year, sonMenuKaydi.Tarih.Month, 1);
            var sonAyinSonu = sonAyinBasi.AddMonths(1).AddDays(-1);

            var menuSablonu = await _context.YemekMenuleri
                                            .Where(m => m.Tarih >= sonAyinBasi && m.Tarih <= sonAyinSonu && m.AnaYemek != "Yemekhane Kapal�")
                                            .OrderBy(m => m.Tarih)
                                            .ToListAsync();

            // E�er �ablon olu�turulamad�ysa (veri yoksa) i�lemi durdur.
            if (!menuSablonu.Any())
            {
                return;
            }

            // Ad�m 3: ��inde bulundu�umuz ay i�in yeni men� listesi olu�tur.
            var yeniMenuKayitlari = new List<YemekMenu>();
            var ayinIlkGunu = new DateTime(bugun.Year, bugun.Month, 1);
            var aydakiGunSayisi = DateTime.DaysInMonth(bugun.Year, bugun.Month);
            int sablonIndex = 0; // �ablonda hangi yeme�i kullanaca��m�z� takip etmek i�in saya�.

            for (int i = 0; i < aydakiGunSayisi; i++)
            {
                var oGun = ayinIlkGunu.AddDays(i);
                var yeniMenu = new YemekMenu { Tarih = oGun };

                // E�er g�n Pazar ise, "Yemekhane Kapal�" olarak ayarla.
                if (oGun.DayOfWeek == DayOfWeek.Sunday)
                {
                    yeniMenu.Corba = "---";
                    yeniMenu.AnaYemek = "Yemekhane Kapal�";
                    yeniMenu.Tatli = "---";
                }
                else // G�n Pazar de�ilse, �ablondan s�radaki yeme�i ata.
                {
                    var sablondakiYemek = menuSablonu[sablonIndex];
                    yeniMenu.Corba = sablondakiYemek.Corba;
                    yeniMenu.AnaYemek = sablondakiYemek.AnaYemek;
                    yeniMenu.Tatli = sablondakiYemek.Tatli;

                    // D�ng�sel olarak bir sonraki �ablon yeme�ine ge�.
                    sablonIndex = (sablonIndex + 1) % menuSablonu.Count;
                }
                yeniMenuKayitlari.Add(yeniMenu);
            }

            // Ad�m 4: Olu�turulan yeni ay�n men�s�n� veritaban�na toplu olarak ekle.
            await _context.YemekMenuleri.AddRangeAsync(yeniMenuKayitlari);
            await _context.SaveChangesAsync();
        }
    }
}