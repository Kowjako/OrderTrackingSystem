using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OrderTrackingSystem.Logic.EnumMappers;
using System.Data.Entity;
using System;
using System.Transactions;

namespace OrderTrackingSystem.Logic.Services
{
    public class OrderService : IService<OrderService>
    {
        private ProductService ProductService => new ProductService();

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
                                Oplata = EnumConverter.GetNameById<PayType>(order.PayType),
                                Sklep = sellerQuery.First(),
                                Dostawa = EnumConverter.GetNameById<DeliveryType>(order.DeliveryType),
                                Rezygnacja = order.ComplaintDefinitionId != null ? "Tak" : "Nie",
                                Kwota = string.Format("{0:0.00 zł}", valueQuery)
                            };

                return query.ToList();
            }
        }

        public async Task SaveOrder(OrderDTO order, List<CartProductDTO> products)
        {
            var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            using(var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {

                    var orderDAL = new Orders
                    {
                        Number = ConfigurationService.GenerateElementNumber(),
                        CustomerId = order.CustomerId,
                        PayType = byte.Parse(order.Oplata),
                        DeliveryType = byte.Parse(order.Dostawa),
                        PickupId = order.PickupId,
                        SellerId = order.SellerId,
                        OrderDate = DateTime.Now,
                        ComplaintDefinitionId = null
                    };
                    dbContext.Orders.Add(orderDAL);
                    /* Zapisujemy zamówienie */
                    await dbContext.SaveChangesAsync();
                    /* Zapisujemy subelementy */
                    await ProductService.SaveOrderProductsForCart(products, orderDAL.Id);
                    /* Nadawanie statusu */
                    dbContext.OrderStates.Add(new OrderStates
                    {
                        OrderId = orderDAL.Id,
                        Date = DateTime.Now,
                        State = "W trakcie przygotowania",
                        Description = "Przesyłka jest przygotowywana przez producenta"
                    });
                    await dbContext.SaveChangesAsync();
                }
                transactionScope.Complete();
            }
        }

        public async Task<List<Orders>> GetOrdersListByCodes(string[] codes)
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var orderQuery = from order in dbContext.Orders
                                 where codes.Contains(order.Number)
                                 select order;
                return await orderQuery.AsNoTracking().ToListAsync();
            }
        }
    }
}
