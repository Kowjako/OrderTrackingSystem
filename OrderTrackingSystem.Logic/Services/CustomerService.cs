using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
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

        public async void UpdateCustomer(Customers customer)
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<CustomerDTO> GetCustomer(int customerId, bool isOrder)
        {
            return new CustomerDTO();
        }
    }
}
