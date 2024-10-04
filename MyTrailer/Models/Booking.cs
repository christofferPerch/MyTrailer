namespace MyTrailer.Models {
    public class Booking {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public int TrailerId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsInsured { get; set; }
        public bool IsOverdue { get; set; }
        public decimal LateFee { get; set; }  
        public bool IsOverNight { get; set; } 
        public decimal OverNightFee { get; set; }
        public DateTime? ActualReturnTime { get; set; } 

    }
}
