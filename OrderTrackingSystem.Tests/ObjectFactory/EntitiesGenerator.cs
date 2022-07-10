using OrderTrackingSystem.Logic.DataAccessLayer;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using System.Threading.Tasks;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;

namespace OrderTrackingSystem.Tests.ObjectFactory
{
    public class EntitiesGenerator
    {
        public IProductService ProductService;
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;

        public EntitiesGenerator()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            ProductService = new ProductService(new ConfigurationService());
        }

        public async Task<Products> CreateProductWithSeller()
        {
            var seller = OF.ObjectFactory.CreateSeller();
            var localization = OF.ObjectFactory.CreateLocalization();
            await LocalizationService.AddNewLocalization(localization);
            await CustomerService.AddNewSeller(seller, localization.Id, ("login", "pass"));

            var product = OF.ObjectFactory.CreateProduct();
            product.SellerId = seller.Id;
            return product;
        }

        public async Task<int> CreateLocalization()
        {
            var localization = OF.ObjectFactory.CreateLocalization();
            await LocalizationService.AddNewLocalization(localization);
            return localization.Id;
        }

        public async Task<Customers> AddNewCustomerToDb()
        {
            var customer = OF.ObjectFactory.CreateCustomer();
            await CustomerService.AddNewCustomer(customer, await CreateLocalization(), ("login", "pass"));
            return customer;
        }

        public async Task<Sellers> AddNewSellerToDb()
        {
            var seller = OF.ObjectFactory.CreateSeller();
            await CustomerService.AddNewSeller(seller, await CreateLocalization(), ("login", "pass"));
            return seller;
        }

        public async Task<Pickups> AddNewPickupToDb()
        {
            var pickup = OF.ObjectFactory.CreatePickup();
            var localizationId = await CreateLocalization();
            pickup.LocalizationId = localizationId;
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Pickups.Add(pickup);
                await dbContext.SaveChangesAsync();
            }

            return pickup;
        }
    }
}
