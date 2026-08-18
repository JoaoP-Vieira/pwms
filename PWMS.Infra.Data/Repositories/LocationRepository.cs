using Dapper;
using PWMS.Core.Entities.Address;
using PWMS.Core.Interfaces.Address;

namespace PWMS.Infra.Data.Repositories
{
	public class LocationRepository : BaseRepository, ILocationRepository
	{
        private const string FIND_AVALIABLE_CONFERENCE_LOCATION = @"SELECT
            l.id AS ""Id"",
            l.identification AS ""Identification"",
            l.""zone"" AS ""Zone"",
            l.is_locked AS ""IsLocked"" 
        FROM ""location"" l
        WHERE l.""zone"" = 0
            AND NOT EXISTS (
                SELECT 1 
                FROM invoice i 
                WHERE i.conference_location_id = l.id 
                    AND i.status IN (0, 1, 2)
            )
        LIMIT 1;";

		public LocationRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task<Location?> GetAvaliableConferenceLocation()
		{
            var conn = _dbContext.GetConnection();

			return await conn.QueryFirstOrDefaultAsync<Location>(FIND_AVALIABLE_CONFERENCE_LOCATION);
		}
	}
}
