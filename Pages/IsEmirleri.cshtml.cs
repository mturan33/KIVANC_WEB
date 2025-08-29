using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KIVANC_WEB.Pages
{
    public class IsEmirleriModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IsEmirleriModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<IsEmri> IsEmriListesi { get; set; } = new();

        public async Task OnGetAsync()
        {
            IsEmriListesi = await _context.IsEmirleri
                                          .OrderByDescending(i => i.OlusturmaTarihi)
                                          .ToListAsync();
        }
    }
}