using Microsoft.AspNetCore.Mvc;
using MyTrailer.Models;
using MyTrailer.Services;
using System.Threading.Tasks;

namespace MyTrailer.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TrailerController : ControllerBase {
        private readonly TrailerListService _trailerService;

        public TrailerController(TrailerListService trailerService) {
            _trailerService = trailerService;
        }

        // Get trailer by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrailerById(int id) {
            var trailer = await _trailerService.GetTrailerById(id);
            if (trailer == null) {
                return NotFound("Trailer not found.");
            }

            return Ok(trailer);
        }

        // Get available trailers by location
        [HttpGet("location/{locationId}")]
        public async Task<IActionResult> GetAvailableTrailersByLocation(int locationId) {
            var trailers = await _trailerService.GetAvailableTrailersByLocation(locationId);
            if (trailers == null || trailers.Count == 0) {
                return NotFound("No available trailers at this location.");
            }

            return Ok(trailers);
        }

        // Add a new trailer
        [HttpPost]
        public async Task<IActionResult> AddTrailer([FromBody] Trailer trailer) {
            if (trailer == null) {
                return BadRequest("Invalid trailer data.");
            }

            var newTrailerId = await _trailerService.AddTrailer(trailer);
            return CreatedAtAction(nameof(GetTrailerById), new { id = newTrailerId }, trailer);
        }

        // Update trailer details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrailer(int id, [FromBody] Trailer trailer) {
            if (trailer == null || id != trailer.Id) {
                return BadRequest("Invalid trailer data.");
            }

            await _trailerService.UpdateTrailer(trailer);
            return NoContent();
        }

        // Delete a trailer by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrailer(int id) {
            await _trailerService.DeleteTrailer(id);
            return NoContent();
        }

        // Update trailer availability
        [HttpPatch("{id}/availability")]
        public async Task<IActionResult> UpdateTrailerAvailability(int id, [FromBody] bool isAvailable) {
            await _trailerService.UpdateTrailerAvailability(id, isAvailable);
            return NoContent();
        }
    }
}
