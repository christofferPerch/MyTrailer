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
            var sql = @"SELECT Id, LocationName, Address, PartnerCompany, 
                CASE
                    WHEN LocationName = 'Jem og Fix' THEN '~/images/JemOgFixLogo.jpg'
                    WHEN LocationName = 'Fog' THEN '~/images/FogLogo.jpg'
                    WHEN LocationName = 'Bauhaus' THEN '~/images/BauhausLogo.jpg'
                    WHEN LocationName = 'Silvan' THEN '~/images/SilvanLogo.png'
                    WHEN LocationName = 'Harald Skrald' THEN '~/images/HaraldSkraldLogo.png'
                    ELSE '~/images/locations/default.png'
                END AS BrandingImagePath
                FROM Location";

            return await _dataAccess.GetAll<Location>(sql);
        }


        public async Task<string?> GetLocationName(int locationId)
        {
            var sql = "SELECT LocationName FROM Location WHERE Id = @LocationId";
            var parameters = new { LocationId = locationId };
            return await _dataAccess.GetById<string>(sql, parameters);
        }

        public async Task<string?> GetBrandingImagePath(int locationId)
        {
            var sql = "SELECT BrandingImagePath FROM Location WHERE Id = @LocationId";
            var parameters = new { LocationId = locationId };
            return await _dataAccess.GetById<string>(sql, parameters);
        }

    }
}
