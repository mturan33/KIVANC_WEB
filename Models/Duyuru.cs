namespace KIVANC_WEB.Models
{
    public class Duyuru
    {
        public int Id { get; set; } // Primary Key
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public DateTime YayinTarihi { get; set; }
    }
}
