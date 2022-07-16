using OrderTrackingSystem.Logic.DataAccessLayer;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using System.Threading.Tasks;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System.Linq;
using System.Collections.Generic;

namespace OrderTrackingSystem.Tests.ObjectFactory
{
    public class EntitiesGenerator
    {
        public IProductService ProductService;
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;
        public IMailService MailService;
        public IComplaintService ComplaintService;
        public IOrderService OrderService;

        public EntitiesGenerator()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            ProductService = new ProductService(new ConfigurationService());
            MailService = new MailService();
            OrderService = new OrderService(CustomerService);
            ComplaintService = new ComplaintService();
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
            var receiver = await AddNewCustomerToDb();
            var seller = await AddNewSellerToDb();
            var customer = await AddNewCustomerToDb();
            var product = OF.ObjectFactory.CreateProduct();

            order.SellerId = customer.Id;
            order.CustomerId = receiver.Id;

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

        public async Task<int> AddNewComplaintDefinitionToDb()
        {
            var complaintDefinition = OF.ObjectFactory.CreateComplaintDefinition();
            var folder = "Folder XXL";
            await ComplaintService.AddNewFolder(folder, null);
            var folderId = (await ComplaintService.GetComplaintFolders()).Where(p => p.Name.Equals(folder))
                                                                         .Select(p => p.Id)
                                                                         .First();

            await ComplaintService.SaveComplaintTemplate(complaintDefinition, folderId);
            return complaintDefinition.Id;
        }

        public async Task<(int complaintId, int customerId, int sellerId)> AddNewComplaintToDb()
        {
            (var order, var product, var customer) = await AddNewOrderToDb();
            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };
            await OrderService.SaveOrder(order, elemList, 100.0m);

            var complaintDefinitionId = await AddNewComplaintDefinitionToDb();
            await ComplaintService.RegisterNewComplaint(complaintDefinitionId, order.Id);

            var complaints = await ComplaintService.GetComplaintsForCustomer(customer.Id);
            return (complaints.Select(p => p.Id).First(), customer.Id, product.SellerId);
        }

        public async Task<List<ComplaintFolderDTO>> AddComplaintFoldersTreeToDb(string sep)
        {
            var folder = "Folder 12" + sep;
            await ComplaintService.AddNewFolder(folder, null);
            var folderX = (await ComplaintService.GetComplaintFoldersWithoutComposing()).Where(p => p.Name.Equals(folder))
                                                                        .First();
            var folder1 = "Folder 12.1" + sep;
            var folder2 = "Folder 12.2" + sep;
            await ComplaintService.AddNewFolder(folder1, folderX);
            await ComplaintService.AddNewFolder(folder2, folderX);

            var folderY = (await ComplaintService.GetComplaintFoldersWithoutComposing()).Where(p => p.Name.Equals(folder2))
                                                                        .First();
            var folder3 = "Folder 12.2.1" + sep;
            var folder4 = "Folder 12.2.2" + sep;
            var folder5 = "Folder 12.2.3" + sep;
            await ComplaintService.AddNewFolder(folder3, folderY);
            await ComplaintService.AddNewFolder(folder4, folderY);
            await ComplaintService.AddNewFolder(folder5, folderY);

            return await ComplaintService.GetComplaintFolders();
        }
    }
}
