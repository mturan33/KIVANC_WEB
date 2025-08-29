using KIVANC_WEB.Data;
using KIVANC_WEB.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KIVANC_WEB.Pages
{
    public class PersonelRehberiModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PersonelRehberiModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Personel> PersonelListesi { get; set; } = new();

        public async Task OnGetAsync()
        {
            PersonelListesi = await _context.PersonelRehberi
                                            .OrderBy(p => p.AdSoyad)
                                            .ToListAsync();
        }
    }
}