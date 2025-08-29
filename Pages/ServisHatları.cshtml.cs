using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KIVANC_WEB.Pages
{
    public class ServisHatlarıModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ServisHatlarıModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ServisHatti> ServisHattiListesi { get; set; } = new();

        public async Task OnGetAsync()
        {
            ServisHattiListesi = await _context.ServisHatları
                                               .OrderBy(s => s.HatAdi) // Hat adına göre sırala
                                               .ToListAsync();
        }
    }
}