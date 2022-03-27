using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

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

        public async Task<CustomerDTO> GetCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                IQueryable<CustomerDTO> query = null;
                query = from customer in dbContext.Customers
                        let localizationQuery = (from localization in dbContext.Localizations
                                                where localization.Id == customer.LocalizationId
                                                select localization).FirstOrDefault()
                        where customer.Id == customerId

                        select new CustomerDTO
                        {
                            Id = customer.Id,
                            Nazwa = customer.Name + " " + customer.Surname,
                            Adres = localizationQuery.Street + " " +
                                    localizationQuery.House + ", " +
                                    localizationQuery.Flat,
                            Email = customer.Email,
                            MiastoKod = localizationQuery.City + ", " +
                                        localizationQuery.ZipCode,
                            Numer = customer.Number
                        };
                return await query.FirstAsync();
            }
        }

        public async Task<CustomerDTO> GetSeller(int sellerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                IQueryable<CustomerDTO> query = null;
                query = from seller in dbContext.Sellers
                        let localizationQuery = (from localization in dbContext.Localizations
                                                where localization.Id == seller.LocalizationId
                                                select localization).FirstOrDefault()
                        where seller.Id == sellerId

                        select new CustomerDTO
                        {
                            Nazwa = seller.Name,
                            Adres = localizationQuery.Street + " " +
                                    localizationQuery.House + ", " +
                                    localizationQuery.Flat,
                            Email = seller.Email,
                            MiastoKod = localizationQuery.City + ", " +
                                        localizationQuery.ZipCode,
                            Numer = seller.Number
                        };
                return await query.FirstAsync();
            }
        }
    }
}
