using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
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
            var sql = @"SELECT * FROM Trailers WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Trailer>(sql, parameters);
        }

        public async Task<List<Trailer>> GetAllTrailers()
        {
            var sql = @"SELECT * FROM Trailers";
            return await _dataAccess.GetAll<Trailer>(sql);
        }

        public async Task<int> AddTrailer(Trailer trailer)
        {
            var sql = @"INSERT INTO Trailers (LocationId, Number, IsAvailable)
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
            var sql = @"UPDATE Trailers
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

        public async Task DeleteTrailer(int id)
        {
            var sql = @"DELETE FROM Trailers WHERE Id = @Id";
            var parameters = new { Id = id };
            await _dataAccess.Delete(sql, parameters);
        }
    }
}
