using DataAccess;
using BookingService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Services
{
    public class TrailerService
    {
        private readonly IDataAccess _dataAccess;

        public TrailerService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task<Trailer?> GetTrailerById(int id)
        {
            var sql = @"SELECT * FROM Trailer WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Trailer>(sql, parameters);
        }

        public async Task<List<Trailer>> GetAvailableTrailersByLocation(int locationId)
        {
            var sql = @"SELECT * FROM Trailer WHERE LocationId = @LocationId AND IsAvailable = 1";
            var parameters = new { LocationId = locationId };
            return await _dataAccess.GetAll<Trailer>(sql, parameters);
        }

        public async Task<int> AddTrailer(Trailer trailer)
        {
            var sql = @"INSERT INTO Trailer (LocationId, Number, IsAvailable)
                        OUTPUT INSERTED.Id
                        VALUES (@LocationId, @Number, @IsAvailable)";

            var parameters = new
            {
                trailer.LocationId,
                trailer.Number,
                trailer.IsAvailable
            };

            return await _dataAccess.InsertAndGetId<int>(sql, parameters);
        }

        public async Task UpdateTrailer(Trailer trailer)
        {
            var sql = @"UPDATE Trailer
                        SET LocationId = @LocationId, Number = @Number, IsAvailable = @IsAvailable
                        WHERE Id = @Id";

            var parameters = new
            {
                trailer.LocationId,
                trailer.Number,
                trailer.IsAvailable,
                trailer.Id
            };

            await _dataAccess.Update(sql, parameters);
        }

        public async Task UpdateTrailerAvailability(int trailerId, bool isAvailable)
        {
            var sql = @"UPDATE Trailer SET IsAvailable = @IsAvailable WHERE Id = @Id";
            var parameters = new { Id = trailerId, IsAvailable = isAvailable };
            await _dataAccess.Update(sql, parameters);
        }

        public async Task DeleteTrailer(int id)
        {
            var sql = @"DELETE FROM Trailer WHERE Id = @Id";
            var parameters = new { Id = id };
            await _dataAccess.Delete(sql, parameters);
        }
    }
}
