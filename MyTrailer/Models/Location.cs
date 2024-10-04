namespace MyTrailer.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string PartnerCompany { get; set; }
        public string BrandingImagePath { get; set; } 
        public List<Trailer> Trailers { get; set; } = new List<Trailer>(); 
    }
}
