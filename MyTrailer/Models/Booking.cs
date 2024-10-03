namespace MyTrailer.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int TrailerId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsInsured { get; set; }
        public bool IsOverdue { get; set; }
        public Insurance Insurance { get; set; } // Each booking can have one insurance
    }
}
