using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
{
    public class BookingService
    {
        private readonly IDataAccess _dataAccess;

        public BookingService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
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
    }
}
