using Microsoft.AspNetCore.Mvc;
using BookingService.Models;
using MyTrailer.Services;

namespace BookingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingTrailerService _bookingService;
        private readonly TrailerService _trailerService;

        public BookingController(BookingTrailerService bookingService, TrailerService trailerService)
        {
            _bookingService = bookingService;
            _trailerService = trailerService;
        }                                                                                                   

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            var trailer = await _trailerService.GetTrailerById(booking.TrailerId);
            if (trailer == null || !trailer.IsAvailable)
            {
                return BadRequest("Trailer is not available.");
            }

            var bookingId = await _bookingService.AddBooking(booking);

            trailer.IsAvailable = false;
            await _trailerService.UpdateTrailer(trailer);

            return Ok(new { BookingId = bookingId });
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnTrailer(int id, [FromBody] DateTime actualReturnTime)
        {
            try
            {
                await _bookingService.ProcessReturn(id, actualReturnTime);
                return Ok("Trailer returned successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(string userId)
        {
            var bookings = await _bookingService.GetUserBookings(userId);
            return Ok(bookings);
        }
    }
}
