namespace KIVANC_WEB.Models
{
    public class ServisHatti
    {
        public int Id { get; set; }
        public string HatAdi { get; set; } // Örn: Balcalı - Turgut Özal Hattı
        public string Guzergah { get; set; } // Örn: Durak 1 - Durak 2 - Durak 3...
        public string SoforAdi { get; set; }
        public string Plaka { get; set; }
    }
}