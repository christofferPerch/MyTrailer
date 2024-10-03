namespace MyTrailer.Models {
    public class Booking {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int TrailerId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsInsured { get; set; }
        public bool IsOverdue { get; set; } // Indicates if the trailer was returned late
        public decimal LateFee { get; set; } // Stores the calculated late fee if applicable
        public Insurance Insurance { get; set; } // Each booking can have one insurance
    }
}
