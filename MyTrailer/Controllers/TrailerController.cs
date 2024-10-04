using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTrailer.Models;
using MyTrailer.Services;
using MyTrailer.ViewModels;
using System.Security.Claims;




namespace MyTrailer.Controllers
{
    public class TrailerController : Controller
    {
        private readonly TrailerService _trailerService;
        private readonly LocationService _locationService;
        private readonly BookingService _bookingService;
        private readonly UserManager<IdentityUser> _userManager;

        public TrailerController(TrailerService trailerService, LocationService locationService, BookingService bookingService, UserManager<IdentityUser> userManager)
        {
            _trailerService = trailerService;
            _locationService = locationService;
            _bookingService = bookingService;
            _userManager = userManager;
        }

        public string? GetUserId(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int trailerId, int customerId, DateTime startDateTime, DateTime endDateTime, bool isInsured)
        {
            var trailer = await _trailerService.GetTrailerById(trailerId);
            if (trailer == null || !trailer.IsAvailable)
            {
                return BadRequest("Trailer is not available.");
            }

            bool isOverNight = startDateTime.Date != endDateTime.Date;
            decimal overNightFee = isOverNight ? 500m : 0m;
            var userId = GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var booking = new Booking
            {
                UserId = userId,
                TrailerId = trailerId,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                IsInsured = isInsured,
                IsOverdue = false,
                IsOverNight = isOverNight,
                OverNightFee = overNightFee
            };

            var bookingId = await _bookingService.AddBooking(booking);

            trailer.IsAvailable = false;
            await _trailerService.UpdateTrailer(trailer);

            if (isOverNight)
            {
                TempData["Message"] = "Overnight booking confirmed with an additional fee of " + overNightFee.ToString("C");
            }

            return RedirectToAction("BookingConfirmed", new { bookingId });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmBooking(int trailerId)
        {
            var trailer = await _trailerService.GetTrailerById(trailerId);
            if (trailer == null || !trailer.IsAvailable)
            {
                return NotFound("Trailer not available.");
            }

            var locationName = await _locationService.GetLocationName(trailer.LocationId);

            var brandingImagePath = await _locationService.GetBrandingImagePath(trailer.LocationId);

            var userId = GetUserId(User); 
            if (userId == null)
            {
                return Unauthorized(); 
            }
            var viewModel = new BookingViewModel
            {
                TrailerId = trailer.Id,
                UserId = userId,
                TrailerNumber = trailer.Number,
                LocationName = locationName,
                BrandingImagePath = brandingImagePath 
            };

            return View(viewModel);
        }


        public IActionResult BookingConfirmed(int bookingId)
        {
            var booking = _bookingService.GetBookingById(bookingId).Result;
            var trailer = _trailerService.GetTrailerById(booking.TrailerId).Result;

            ViewBag.BookingId = bookingId;
            ViewBag.TrailerNumber = trailer.Number;
            ViewBag.StartDateTime = booking.StartDateTime;
            ViewBag.EndDateTime = booking.EndDateTime;
            ViewBag.IsInsured = booking.IsInsured;
            ViewBag.IsOverNight = booking.IsOverNight;
            ViewBag.OverNightFee = booking.OverNightFee;

            return View();
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
        [HttpPost]
        public async Task<IActionResult> ReturnTrailer(int bookingId, DateTime actualReturnTime)
        {
            try
            {
                await _bookingService.ProcessReturn(bookingId, actualReturnTime);
                return RedirectToAction("ReturnSuccess");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult ReturnSuccess()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ReturnTrailerForm(int bookingId)
        {
            var booking = await _bookingService.GetBookingById(bookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            var viewModel = new BookingViewModel
            {
                Id = booking.Id,
                TrailerId = booking.TrailerId,
                TrailerNumber = (await _trailerService.GetTrailerById(booking.TrailerId)).Number,
                StartDateTime = booking.StartDateTime,
                EndDateTime = booking.EndDateTime
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var userId = GetUserId(User); // Get UserId
            if (userId == null)
            {
                return Unauthorized();
            }

            var bookings = await _bookingService.GetUserBookings(userId); // Changed to use UserId
            var bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings)
            {
                var trailer = await _trailerService.GetTrailerById(booking.TrailerId);
                var locationName = await _locationService.GetLocationName(trailer.LocationId);

                bookingViewModels.Add(new BookingViewModel
                {
                    Id = booking.Id,
                    TrailerId = booking.TrailerId,
                    UserId = booking.UserId,
                    TrailerNumber = trailer.Number,
                    StartDateTime = booking.StartDateTime,
                    EndDateTime = booking.EndDateTime,
                    LocationName = locationName,
                    IsOverdue = booking.IsOverdue
                });
            }

            return View(bookingViewModels);
        }
    }
}
