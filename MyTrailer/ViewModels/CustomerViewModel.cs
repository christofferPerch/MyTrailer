namespace MyTrailer.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();
    }
}
