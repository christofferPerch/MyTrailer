namespace MyTrailer.ViewModels
{
    public class BookingViewModel
    {
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
        public string TrailerNumber { get; set; } 
        public string LocationName { get; set; }
        public string BrandingImagePath { get; set; }

    }
}
