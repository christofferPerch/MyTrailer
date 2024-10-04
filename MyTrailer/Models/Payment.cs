namespace MyTrailer.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Method { get; set; } 
        public string History { get; set; } 
    }
}
