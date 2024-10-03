namespace MyTrailer.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public int BookingId { get; set; } 
        public decimal Fee { get; set; } = 50m;
        public string CoveragePeriod { get; set; } 
    }
}
