namespace BookingService.Models
{
    public class Trailer
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Number { get; set; }
        public bool IsAvailable { get; set; }
    }
}
