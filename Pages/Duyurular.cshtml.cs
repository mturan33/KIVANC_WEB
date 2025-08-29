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

        // HTML tarafýnda kullanmak için duyurularýn listesini tutacak bir özellik.
        public List<Duyuru> DuyuruListesi { get; set; } = new();

        // Sayfa yüklendiðinde çalýþacak metot.
        public async Task OnGetAsync()
        {
            // Veritabanýndaki Duyurular tablosundan tüm kayýtlarý,
            // en yeniden en eskiye doðru (YayinTarihi'ne göre azalan sýrada) sýralayarak çekiyoruz.
            DuyuruListesi = await _context.Duyurular
                                          .OrderByDescending(d => d.YayinTarihi)
                                          .ToListAsync();
        }
    }
}