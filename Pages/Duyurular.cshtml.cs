using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KIVANC_WEB.Pages
{
    public class DuyurularModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DuyurularModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // HTML taraf�nda kullanmak i�in duyurular�n listesini tutacak bir �zellik.
        public List<Duyuru> DuyuruListesi { get; set; } = new();

        // Sayfa y�klendi�inde �al��acak metot.
        public async Task OnGetAsync()
        {
            // Veritaban�ndaki Duyurular tablosundan t�m kay�tlar�,
            // en yeniden en eskiye do�ru (YayinTarihi'ne g�re azalan s�rada) s�ralayarak �ekiyoruz.
            DuyuruListesi = await _context.Duyurular
                                          .OrderByDescending(d => d.YayinTarihi)
                                          .ToListAsync();
        }
    }
}