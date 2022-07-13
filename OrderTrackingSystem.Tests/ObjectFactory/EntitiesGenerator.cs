using OrderTrackingSystem.Logic.DataAccessLayer;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using System.Threading.Tasks;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;

namespace OrderTrackingSystem.Tests.ObjectFactory
{
    public class EntitiesGenerator
    {
        public IProductService ProductService;
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;
        public IMailService MailService;

        public EntitiesGenerator()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            ProductService = new ProductService(new ConfigurationService());
            MailService = new MailService();
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

        public async Task<(OrderDTO, Products, Customers)> AddNewOrderToDb()
        {
            var order = OF.ObjectFactory.CreateOrder();
            var pickup = await AddNewPickupToDb();
            var seller = await AddNewSellerToDb();
            var customer = await AddNewCustomerToDb();
            var product = OF.ObjectFactory.CreateProduct();

            order.SellerId = seller.Id;
            order.CustomerId = customer.Id;
            order.PickupId = pickup.Id;

            product.SellerId = seller.Id;
            await ProductService.SaveNewProduct(product);

            return (order, product, customer);
        }

        public async Task<(SellDTO, Products, Customers)> AddNewSellToDb()
        {
            var order = OF.ObjectFactory.CreateSell();
            var seller = await AddNewSellerToDb();
            var customer = await AddNewCustomerToDb();
            var product = OF.ObjectFactory.CreateProduct();

            order.SellerId = seller.Id;
            order.CustomerId = customer.Id;

            product.SellerId = seller.Id;
            await ProductService.SaveNewProduct(product);

            return (order, product, customer);
        }

        public async Task<(Mails, int receiverId, int senderId)> AddNewMailToDbCusToSeller()
        {
            var mail = OF.ObjectFactory.CreateMail();
            var customer = await AddNewCustomerToDb();
            var seller = await AddNewSellerToDb();

            mail.SenderId = customer.Id;
            mail.ReceiverId = seller.Id;
            mail.MailRelation = (int)MailDirectionType.CustomerToSeller;

            await MailService.AddNewMail(mail);
            return (mail, mail.SenderId, mail.ReceiverId);
        }

        public async Task<(Mails, int receiverId, int senderId)> AddNewMailToDbSellerToCus()
        {
            var mail = OF.ObjectFactory.CreateMail();
            var customer = await AddNewCustomerToDb();
            var seller = await AddNewSellerToDb();

            mail.SenderId = seller.Id;
            mail.ReceiverId = customer.Id;
            mail.MailRelation = (int)MailDirectionType.SellerToCustomer;

            await MailService.AddNewMail(mail);
            return (mail, mail.SenderId, mail.ReceiverId);
        }
    }
}
