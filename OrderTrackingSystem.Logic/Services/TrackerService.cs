using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.DTO.Pagination;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class TrackerService : CRUDManager, ITrackerService
    {
        private ConfigurationService ConfigurationService => new ConfigurationService();

        public async Task<List<TrackableItemDTO>> GetItemsForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                /* Ładujemy customer'a*/
                var customer = await dbContext.Customers.Where(c => c.Id == customerId).FirstAsync();

                /* Selekcja DTO dla zamówień */
                var orderQuery = from order in dbContext.Orders
                                 let sellerQuery = from sellers in dbContext.Sellers
                                                   where sellers.Id == order.SellerId
                                                   select sellers
                                 let valueQuery =  from cart in dbContext.OrderCarts
                                                   from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                                   where cart.OrderId == order.Id
                                                   select cart.Amount * prodcut.PriceBrutto
                                 where order.CustomerId == customer.Id
                                 select new TrackableItemDTO()
                                 {
                                     Id = order.Id,
                                     Number = order.Number,
                                     Date = order.OrderDate.Value,
                                     Seller = sellerQuery.FirstOrDefault().Name,
                                     Customer = customer.Name + " " + customer.Surname,
                                     Value = valueQuery.Sum(),
                                     IsOrder = true,
                                     CustomerId = customer.Id,
                                     SellerId = sellerQuery.FirstOrDefault().Id
                                 };

                /* Selekcja DTO dla wysyłek */
                var sendsQuery = from sells in dbContext.Sells
                                 let receiverQuery = from receiver in dbContext.Customers
                                                     where receiver.Id == sells.CustomerId
                                                     select receiver
                                 let valueQuery = from cart in dbContext.SellCarts
                                                   from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                                   where cart.SellId == sells.Id
                                                   select cart.Amount * prodcut.PriceBrutto
                                 where sells.SellerId == customer.Id
                                 select new TrackableItemDTO()
                                 {
                                     Id = sells.Id,
                                     Number = sells.Number,
                                     Date = sells.SellingDate,
                                     Seller = receiverQuery.FirstOrDefault().Name + " " + receiverQuery.FirstOrDefault().Surname,
                                     Customer = customer.Name + " " + customer.Surname,
                                     Value = valueQuery.Sum(),
                                     IsOrder = false,
                                     CustomerId = receiverQuery.FirstOrDefault().Id,
                                     SellerId = customer.Id
                                 };

                /* Union dwóch kolekcji */
                return Paginator.GetPaginatedList(orderQuery.Union(sendsQuery)).ToList();
            }
        }

        public async Task<List<ParcelStateDTO>> GetParcelState(int orderId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var order = await dbContext.Orders.FindAsync(orderId);
                List<OrderStates> OrderStates = null;
                if (order != null)
                {
                    OrderStates = await dbContext.OrderStates.Where(p => p.OrderId == order.Id)
                                                                 .OrderBy(p => p.Date)
                                                                 .ToListAsync();
                }
                List<ParcelStateDTO> ParcelStates = OrderStates.Select(p => new ParcelStateDTO
                {
                    StateId = p.State,
                    Name = ConfigurationService.GetStatusDetails((OrderState)p.State).name,
                    Description = ConfigurationService.GetStatusDetails((OrderState)p.State).description,
                    Date = p.Date
                }).ToList();
                return ParcelStates;
            }
        }

        public async Task AddNewStateForOrder(int orderId, OrderState newState)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var stateId = (int)newState;
                var orderStateDAL = new OrderStates()
                {
                    OrderId = orderId,
                    State = stateId,
                    Date = DateTime.Now
                };

                await base.AddEntity(orderStateDAL);
            }
        }

    }
}
