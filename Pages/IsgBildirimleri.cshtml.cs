using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KIVANC_WEB.Pages
{
    public class IsgBildirimleriModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IsgBildirimleriModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<IsgBildirim> BildirimListesi { get; set; } = new();

        public async Task OnGetAsync()
        {
            BildirimListesi = await _context.IsgBildirimleri
                                            .OrderByDescending(b => b.BildirimTarihi)
                                            .ToListAsync();
        }
    }
}