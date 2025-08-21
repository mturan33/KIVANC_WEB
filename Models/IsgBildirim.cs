namespace KIVANC_WEB.Models
{
    public class IsgBildirim
    {
        public int Id { get; set; }
        public string BildirenKisi { get; set; }
        public string Konum { get; set; }
        public string TehlikeAciklamasi { get; set; }
        public DateTime BildirimTarihi { get; set; }
        public bool CozulduMu { get; set; }
    }
}
