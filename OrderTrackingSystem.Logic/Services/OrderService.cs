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
        public async Task<List<OrderDTO>> GetOrdersForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.Where(c => c.Id == customerId).FirstAsync();
                /* ladujemy kolekcje zamowien dla danego kleinta */
                await dbContext.Entry(customer).Collection(nameof(customer.Orders)).LoadAsync();

                var query = from order in customer.Orders
                            let valueQuery = (from cart in dbContext.OrderCarts
                                              from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                              where cart.OrderId == order.Id
                                              select cart.Amount * prodcut.PriceBrutto).Sum()
                            let sellerQuery = from cart in dbContext.OrderCarts
                                              from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                              from seller in dbContext.Sellers.Where(p => p.Id == prodcut.SellerId).DefaultIfEmpty()
                                              where cart.OrderId == order.Id
                                              select seller.Name
                            select new OrderDTO
                            {
                                Numer = order.Number,
                                Oplata = PayTypeEnumConverter.GetNameById(order.Id),
                                Sklep = sellerQuery.FirstAsync().Result,
                                Dostawa = order.DeliveryType,
                                Rezygnacja = order.ComplaintDefinitionId != null,
                                Kwota = string.Format("{0:0.00 zł}", valueQuery)
                            };

                return query.ToList();
            }
        }
    }
}
