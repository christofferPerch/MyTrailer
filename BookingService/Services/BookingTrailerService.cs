using BookingService.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
{
    public class BookingTrailerService
    {
        private readonly IDataAccess _dataAccess;
        private readonly TrailerService _trailerService;

        public BookingTrailerService(IDataAccess dataAccess, TrailerService trailerService)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            _trailerService = trailerService;
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            var sql = @"SELECT * FROM Booking WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Booking>(sql, parameters);
        }

        public async Task<int> AddBooking(Booking booking)
        {
            var sql = @"INSERT INTO Booking (UserId, TrailerId, StartDateTime, EndDateTime, IsInsured, IsOverNight, OverNightFee, IsOverDue)
                    OUTPUT INSERTED.Id
                    VALUES (@UserId, @TrailerId, @StartDateTime, @EndDateTime, @IsInsured, @IsOverNight, @OverNightFee, @IsOverDue)";

            var parameters = new
            {
                booking.UserId,
                booking.TrailerId,
                booking.StartDateTime,
                booking.EndDateTime,
                booking.IsInsured,
                booking.IsOverNight,
                booking.OverNightFee,
                booking.IsOverdue
            };

            return await _dataAccess.InsertAndGetId<int>(sql, parameters);
        }

        public async Task ProcessReturn(int bookingId, DateTime actualReturnTime)
        {
            var booking = await GetBookingById(bookingId);
            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            bool isLate = actualReturnTime > booking.EndDateTime;
            decimal lateFee = 0m;

            if (isLate)
            {
                booking.IsOverdue = true;
                lateFee = CalculateLateFee(actualReturnTime, booking.EndDateTime);
                booking.LateFee = lateFee;
            }

            var sql = @"UPDATE Booking
                        SET ActualReturnTime = @ActualReturnTime, IsOverdue = @IsOverdue, LateFee = @LateFee
                        WHERE Id = @Id";

            var parameters = new
            {
                ActualReturnTime = actualReturnTime,
                IsOverdue = booking.IsOverdue,
                LateFee = booking.LateFee,
                Id = booking.Id
            };

            await _dataAccess.Update(sql, parameters);

            // Mark trailer as available
            await _trailerService.UpdateTrailerAvailability(booking.TrailerId, true);
        }

        public async Task<List<Booking>> GetUserBookings(string userId)
        {
            var sql = @"SELECT * FROM Booking WHERE UserId = @UserId AND ActualReturnTime IS NULL";
            var parameters = new { UserId = userId };
            return await _dataAccess.GetAll<Booking>(sql, parameters);
        }

        private decimal CalculateLateFee(DateTime actualReturnTime, DateTime expectedReturnTime)
        {
            TimeSpan difference = actualReturnTime - expectedReturnTime;
            return (decimal)difference.TotalHours * 50;  // Assume 50 per hour
        }
    }
}
