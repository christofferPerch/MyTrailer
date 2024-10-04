using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services {
    public class TrailerListService {
        private readonly IDataAccess _dataAccess;

        public TrailerListService(IDataAccess dataAccess) {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        // Get a trailer by its ID
        public async Task<Trailer?> GetTrailerById(int id) {
            var sql = @"SELECT * FROM Trailer WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Trailer>(sql, parameters);
        }

        // Get all trailers
        public async Task<List<Trailer>> GetAllTrailers() {
            var sql = @"SELECT * FROM Trailer";
            return await _dataAccess.GetAll<Trailer>(sql);
        }

        // Add a new trailer to the database
        public async Task<int> AddTrailer(Trailer trailer) {
            var sql = @"INSERT INTO Trailer (LocationId, Number, IsAvailable)
                        OUTPUT INSERTED.Id
                        VALUES (@LocationId, @Number, @IsAvailable)";

            var parameters = new {
                trailer.LocationId,
                trailer.Number,
                trailer.IsAvailable
            };

            return await _dataAccess.InsertAndGetId<int>(sql, parameters);
        }

        // Update an existing trailer's details
        public async Task UpdateTrailer(Trailer trailer) {
            var sql = @"UPDATE Trailer
                        SET LocationId = @LocationId, Number = @Number, IsAvailable = @IsAvailable
                        WHERE Id = @Id";

            var parameters = new {
                trailer.LocationId,
                trailer.Number,
                trailer.IsAvailable,
                trailer.Id
            };

            await _dataAccess.Update(sql, parameters);
        }

        // Delete a trailer by its ID
        public async Task DeleteTrailer(int id) {
            var sql = @"DELETE FROM Trailer WHERE Id = @Id";
            var parameters = new { Id = id };
            await _dataAccess.Delete(sql, parameters);
        }

        // Get all available trailers at a specific location
        public async Task<List<Trailer>> GetAvailableTrailersByLocation(int locationId) {
            var sql = "SELECT * FROM Trailer WHERE LocationId = @LocationId AND IsAvailable = 1";
            var parameters = new { LocationId = locationId };
            return await _dataAccess.GetAll<Trailer>(sql, parameters);
        }

        // Update the availability of a specific trailer
        public async Task UpdateTrailerAvailability(int trailerId, bool isAvailable) {
            var sql = @"UPDATE Trailer SET IsAvailable = @IsAvailable WHERE Id = @Id";
            var parameters = new { Id = trailerId, IsAvailable = isAvailable };
            await _dataAccess.Update(sql, parameters);
        }
    }
}
