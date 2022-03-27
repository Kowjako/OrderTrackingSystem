using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OrderTrackingSystem.Logic.EnumMappers;
using System.Data.Entity;

namespace OrderTrackingSystem.Logic.Services
{
    public class OrderService : IService<OrderService>
    {
        private CustomerService CustomerService => new CustomerService();

        public async Task<List<OrderDTO>> GetOrdersForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.Where(c => c.Id == customerId).FirstAsync();
                await dbContext.Entry(customer).Collection(nameof(customer.Orders)).LoadAsync();

                var query = from order in customer.Orders
                            let valueQuery = (from cart in dbContext.OrderCarts
                                              from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                              where cart.OrderId == order.Id
                                              select cart.Amount * prodcut.PriceBrutto).Sum()
                            let sellerQuery = (from seller in dbContext.Sellers
                                              where seller.Id == order.SellerId
                                              select seller.Name).ToList()
                            select new OrderDTO
                            {
                                /*PayTypeEnumConverter.GetNameById(order.Id)*/
                                Numer = order.Number,
                                Oplata = EnumConverter.GetNameById<PayType>(order.Id),
                                Sklep = sellerQuery.First(),
                                Dostawa = EnumConverter.GetNameById<DeliveryType>(order.DeliveryType),
                                Rezygnacja = order.ComplaintDefinitionId != null ? "Tak" : "Nie",
                                Kwota = string.Format("{0:0.00 zł}", valueQuery)
                            };

                return query.ToList();
            }
        }

        public async Task<int> SaveOrder(OrderDTO order)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Orders.Add(new Orders
                {
                    Number = ConfigurationService.GenerateElementNumber(),
                    CustomerId = order.CustomerId,
                    PayType = byte.Parse(order.Oplata),
                    DeliveryType = byte.Parse(order.Dostawa),
                    PickupId = order.PickupId,
                    SellerId = order.SellerId,
                    ComplaintDefinitionId = null
                });
                return await dbContext.SaveChangesAsync();
            }
        }
    }
}
