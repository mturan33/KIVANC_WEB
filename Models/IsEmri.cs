namespace KIVANC_WEB.Models
{
    public class IsEmri
    {
        public int Id { get; set; }
        public string Konu { get; set; }
        public string AtananKisi { get; set; }
        public string Lokasyon { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? TamamlanmaTarihi { get; set; } // Null olabilir, henüz tamamlanmadıysa boştur.
        public bool TamamlandiMi { get; set; }
    }
}