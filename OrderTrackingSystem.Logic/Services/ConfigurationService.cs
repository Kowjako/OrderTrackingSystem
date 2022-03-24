using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class ConfigurationService : IService<ConfigurationService>
    {
        public async Task<int?> GetCurrentSessionId()
        {
            var connectionString = @"data source=WLODEKPC\SQLEXPRESS;initial catalog=OrderTrackingSystem;integrated security=True;MultipleActiveResultSets=True";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand("SELECT AccountId, IsClient FROM Session", sqlConnection))
                {
                    await sqlConnection.OpenAsync();
                    using (var sqlReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while(await sqlReader.ReadAsync())
                        {
                            return sqlReader.GetValue(0) as int?;
                        }
                        return -1;
                    }
                }
            }
        }

        public async Task<List<PickupDTO>> GetPickupPoints()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from pickup in dbContext.Pickups
                            let localizationQuery = (from localization in dbContext.Localizations
                                                     where localization.Id == pickup.LocalizationId
                                                     select localization).FirstOrDefault()
                            select new PickupDTO
                            {
                                Capacity = pickup.Capacity.ToString(),
                                Adres = localizationQuery.Street + " " +
                                        localizationQuery.House,
                                MiastoKod = localizationQuery.City + ", " +
                                            localizationQuery.ZipCode,
                                WorkTime = pickup.WorkHours
                            };
                return await query.ToListAsync();
            }
        }

    }
}
