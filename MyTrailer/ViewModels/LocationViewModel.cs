namespace MyTrailer.ViewModels
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string PartnerCompany { get; set; }
        public List<TrailerViewModel> Trailers { get; set; } = new List<TrailerViewModel>();
    }
}
