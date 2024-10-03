using Microsoft.AspNetCore.Mvc;
using MyTrailer.Services;
using System.Threading.Tasks;

namespace MyTrailer.Controllers
{
    public class TrailerController : Controller
    {
        private readonly TrailerService _trailerService;
        private readonly LocationService _locationService;

        public TrailerController(TrailerService trailerService, LocationService locationService)
        {
            _trailerService = trailerService;
            _locationService = locationService;
        }

        public async Task<IActionResult> SelectLocation()
        {
            var locations = await _locationService.GetAllLocations();
            return View(locations);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int locationId)
        {
            var trailers = await _trailerService.GetAvailableTrailersByLocation(locationId);
            ViewBag.LocationName = await _locationService.GetLocationName(locationId); 
            return View(trailers);
        }
    }
}
