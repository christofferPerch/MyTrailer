namespace MyTrailer.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int TrailerId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsInsured { get; set; }
        public bool IsOverdue { get; set; }
        public string CustomerName { get; set; } 
        public string TrailerNumber { get; set; } 
        public string LocationName { get; set; } 
    }
}
