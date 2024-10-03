using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
{
    public class BookingService
    {
        private const decimal LateFeePerHour = 100m;
        private readonly IDataAccess _dataAccess;
        private readonly CustomerService _customerService;

        public BookingService(IDataAccess dataAccess, CustomerService customerService)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            _customerService = customerService;
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            var sql = @"SELECT * FROM Booking WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Booking>(sql, parameters);
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            var sql = @"SELECT * FROM Booking";
            return await _dataAccess.GetAll<Booking>(sql);
        }

        public async Task<int> AddBooking(Booking booking)
        {
            var sql = @"INSERT INTO Booking (CustomerId, TrailerId, StartDateTime, EndDateTime, IsInsured, IsOverdue)
                        OUTPUT INSERTED.Id
                        VALUES (@CustomerId, @TrailerId, @StartDateTime, @EndDateTime, @IsInsured, @IsOverdue)";

            var parameters = new
            {
                booking.CustomerId,
                booking.TrailerId,
                booking.StartDateTime,
                booking.EndDateTime,
                booking.IsInsured,
                booking.IsOverdue
            };

            return await _dataAccess.InsertAndGetId<int>(sql, parameters);
        }

        public async Task UpdateBooking(Booking booking)
        {
            var sql = @"UPDATE Booking
                        SET CustomerId = @CustomerId, TrailerId = @TrailerId, StartDateTime = @StartDateTime,
                            EndDateTime = @EndDateTime, IsInsured = @IsInsured, IsOverdue = @IsOverdue
                        WHERE Id = @Id";

            var parameters = new
            {
                booking.CustomerId,
                booking.TrailerId,
                booking.StartDateTime,
                booking.EndDateTime,
                booking.IsInsured,
                booking.IsOverdue,
                booking.Id
            };

            await _dataAccess.Update(sql, parameters);
        }

        public async Task DeleteBooking(int id)
        {
            var sql = @"DELETE FROM Booking WHERE Id = @Id";
            var parameters = new { Id = id };
            await _dataAccess.Delete(sql, parameters);
        }

        public async Task<List<Booking>> GetCustomerBookings(int customerId) {
            var sql = @"SELECT * FROM Booking WHERE CustomerId = @CustomerId";
            var parameters = new { CustomerId = customerId };
            return await _dataAccess.GetAll<Booking>(sql, parameters);
        }


        public async Task ProcessReturn(int bookingId, DateTime actualReturnTime) {
            // Get booking details
            var booking = await GetBookingById(bookingId);

            if (booking == null)
                throw new ArgumentException("Invalid booking ID");

            // Check if the trailer is returned after the scheduled end time
            if (actualReturnTime > booking.EndDateTime) {
                booking.IsOverdue = true;

                // Calculate the hours late (rounded up)
                var hoursLate = (int)Math.Ceiling((actualReturnTime - booking.EndDateTime).TotalHours);

                // Calculate late fee
                var lateFee = 100m * hoursLate; // Example: 100 Kr per hour
                booking.LateFee = lateFee;

                // Apply late fee to customer account
                await _customerService.ApplyLateFee(1, booking.LateFee);
            }

            // Mark trailer as available again
            var updateTrailerSql = @"UPDATE Trailer SET IsAvailable = 1 WHERE Id = @Id"; // Ensure @Id is declared here
            await _dataAccess.Update(updateTrailerSql, new { Id = booking.TrailerId });

            // Update booking to reflect return and late fee
            var updateBookingSql = @"UPDATE Booking SET IsOverdue = @IsOverdue, LateFee = @LateFee WHERE Id = @Id"; // Ensure @Id is declared here
            await _dataAccess.Update(updateBookingSql, new {
                IsOverdue = booking.IsOverdue,
                LateFee = booking.LateFee,
                Id = booking.Id // Make sure the @Id parameter is passed
            });
        }
    }
}
