using OrderTrackingSystem.Logic.DataAccessLayer;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class CustomerService : IService<Customers>
    {
        public async Task<Customers> GetCurrentCustomer()
        {
            var configrationService = new ConfigurationService();
            var sessionId = await configrationService.GetCurrentSessionId();
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                return await dbContext.Customers.FindAsync(sessionId);
            }
        }
    }
}
