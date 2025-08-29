using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KIVANC_WEB.Models;

namespace KIVANC_WEB.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<YemekMenu> YemekMenuleri { get; set; }
        public DbSet<IsgBildirim> IsgBildirimleri { get; set; }
        public DbSet<IsEmri> IsEmirleri { get; set; }
        public DbSet<ServisHatti> ServisHatları { get; set; }
        public DbSet<Personel> PersonelRehberi { get; set; }

    }
}
