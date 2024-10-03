using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
{
    public class LocationService
    {
        private readonly IDataAccess _dataAccess;

        public LocationService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task<List<Location>> GetAllLocations()
        {
            var sql = "SELECT Id, LocationName, Address, PartnerCompany FROM Location";
            return await _dataAccess.GetAll<Location>(sql);
        }

        public async Task<string?> GetLocationName(int locationId)
        {
            var sql = "SELECT LocationName FROM Location WHERE Id = @LocationId";
            var parameters = new { LocationId = locationId };
            return await _dataAccess.GetById<string>(sql, parameters);
        }
    }
}
