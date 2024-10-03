using Microsoft.AspNetCore.Mvc;
using MyTrailer.Models;
using MyTrailer.Services;
using MyTrailer.ViewModels;
using System.Threading.Tasks;

namespace MyTrailer.Controllers
{
    public class TrailerController : Controller
    {
        private readonly TrailerService _trailerService;
        private readonly LocationService _locationService;
        private readonly BookingService _bookingService;
        private readonly CustomerService _customerService;
        public TrailerController(TrailerService trailerService, LocationService locationService, BookingService bookingService, CustomerService customerService)
        {
            _trailerService = trailerService;
            _locationService = locationService;
            _bookingService = bookingService;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int trailerId, int customerId, DateTime startDateTime, DateTime endDateTime, bool isInsured) {
            // Get the trailer details
            var trailer = await _trailerService.GetTrailerById(trailerId);
            if (trailer == null || !trailer.IsAvailable) {
                return BadRequest("Trailer is not available.");
            }

            // Create a booking
            var booking = new Booking {
                CustomerId = 1,
                TrailerId = trailerId,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                IsInsured = isInsured,
                IsOverdue = false
            };

            // Add the booking to the database
            var bookingId = await _bookingService.AddBooking(booking);

            // Mark the trailer as unavailable
            trailer.IsAvailable = false;
            await _trailerService.UpdateTrailer(trailer);

            return RedirectToAction("BookingConfirmed", new { bookingId });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmBooking(int trailerId) {
            var trailer = await _trailerService.GetTrailerById(trailerId);
            if (trailer == null || !trailer.IsAvailable) {
                return NotFound("Trailer not available.");
            }

            var customerId = 1; // Example customer ID, should be dynamically retrieved (e.g., from session)
            var viewModel = new BookingViewModel {
                TrailerId = trailer.Id,
                CustomerId = customerId,
                TrailerNumber = trailer.Number,
                LocationName = await _locationService.GetLocationName(trailer.LocationId)
            };

            return View(viewModel); // Make sure ConfirmBooking.cshtml exists in Trailer folder
        }
        public IActionResult BookingConfirmed(int bookingId) {
            ViewBag.BookingId = bookingId;
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
        public async Task<IActionResult> ReturnTrailer(int bookingId, DateTime actualReturnTime) {
            try {
                await _bookingService.ProcessReturn(bookingId, actualReturnTime);
                return RedirectToAction("ReturnSuccess");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult ReturnSuccess() {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ReturnTrailerForm(int bookingId) {
            var booking = await _bookingService.GetBookingById(bookingId);
            if (booking == null) {
                return NotFound("Booking not found.");
            }

            var viewModel = new BookingViewModel {
                Id = booking.Id,
                TrailerId = booking.TrailerId,
                TrailerNumber = (await _trailerService.GetTrailerById(booking.TrailerId)).Number,
                StartDateTime = booking.StartDateTime,
                EndDateTime = booking.EndDateTime
            };

            return View(viewModel); // This points to the ReturnTrailerForm.cshtml view
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings(int customerId) {
            var bookings = await _bookingService.GetCustomerBookings(1);
            var bookingViewModels = new List<BookingViewModel>();

            foreach (var booking in bookings) {
                var trailer = await _trailerService.GetTrailerById(booking.TrailerId);
                var locationName = await _locationService.GetLocationName(trailer.LocationId);

                bookingViewModels.Add(new BookingViewModel {
                    Id = booking.Id,
                    TrailerId = booking.TrailerId,
                    CustomerId = 1,
                    TrailerNumber = trailer.Number,
                    StartDateTime = booking.StartDateTime,
                    EndDateTime = booking.EndDateTime,
                    LocationName = locationName,
                    IsOverdue = booking.IsOverdue
                });
            }

            return View(bookingViewModels); // This points to MyBookings.cshtml
        }


    }
}
