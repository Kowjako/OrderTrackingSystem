using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    class ConfigurationService : IService<ConfigurationService>
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

    }
}
