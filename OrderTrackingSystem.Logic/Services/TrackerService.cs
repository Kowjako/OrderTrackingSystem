using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class TrackerService : IService<TrackerService>
    {
        public async Task<List<TrackableItemDTO>> GetItemsForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                /* Ładujemy customer'a wraz z zamówieniami i wysyłkami */
                var customer = await dbContext.Customers.Where(c => c.Id == customerId).Include(p => p.Sells)
                                                                                       .Include(p => p.Orders)
                                                                                       .AsNoTracking()
                                                                                       .FirstAsync();
                /* Selekcja DTO dla zamówień */
                var orderQuery = from order in customer.Orders
                                 let sellerQuery = from sellers in dbContext.Sellers
                                                   where sellers.Id == order.SellerId
                                                   select sellers
                                 let valueQuery = (from cart in dbContext.OrderCarts
                                                   from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                                   where cart.OrderId == order.Id
                                                   select cart.Amount * prodcut.PriceBrutto).Sum()
                                 select new TrackableItemDTO()
                                 {
                                     Id = order.Id,
                                     Numer = order.Number,
                                     Data = order.OrderDate.Value.ToShortDateString(),
                                     Nabywca = customer.Name + " " + customer.Surname,
                                     Sprzedawca = sellerQuery.First().Name,
                                     Kwota = string.Format("{0:0.00 zł}", valueQuery),
                                     IsOrder = true,
                                     CustomerId = customer.Id,
                                     SellerId = sellerQuery.First().Id
                                 };

                /* Selekcja DTO dla wysyłek*/
                var sendsQuery = from sells in customer.Sells
                                 let receiverQuery = from receiver in dbContext.Customers
                                                     where receiver.Id == sells.CustomerId
                                                     select receiver
                                 let valueQuery = (from cart in dbContext.SellCarts
                                                   from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                                   where cart.SellId == sells.Id
                                                   select cart.Amount * prodcut.PriceBrutto).Sum()
                                 select new TrackableItemDTO()
                                 {
                                     Id = sells.Id,
                                     Numer = sells.Number,
                                     Data = sells.SellingDate.ToShortDateString(),
                                     Nabywca = receiverQuery.First().Name + " "+ receiverQuery.First().Surname,
                                     Sprzedawca = customer.Name + " " + customer.Surname,
                                     Kwota = string.Format("{0:0.00 zł}", valueQuery),
                                     IsOrder = false,
                                     CustomerId = receiverQuery.First().Id,
                                     SellerId = customer.Id
                                 };

                /* Union dwóch kolekcji */
                return orderQuery.Union(sendsQuery).ToList();
            }
        }
    }
}
