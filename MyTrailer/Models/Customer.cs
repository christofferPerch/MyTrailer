namespace MyTrailer.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>(); 
    }
}
