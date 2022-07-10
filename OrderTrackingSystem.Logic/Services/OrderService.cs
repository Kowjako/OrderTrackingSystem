using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OrderTrackingSystem.Logic.EnumMappers;
using System.Data.Entity;
using System;
using System.Transactions;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System.Diagnostics;
using OrderTrackingSystem.Logic.HelperClasses;

namespace OrderTrackingSystem.Logic.Services
{
    public class OrderService : IOrderService
    {
        private ProductService ProductService => new ProductService(ConfigurationService);
        private CustomerService CustomerService => new CustomerService(ConfigurationService);
        private IConfigurationService ConfigurationService => new ConfigurationService();

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
                            let stateQuery = (from orderState in dbContext.OrderStates
                                              where orderState.OrderId == order.Id
                                              select orderState.State).MaxAsync() /* najpozniejszy status */
                            select new OrderDTO
                            {
                                /*PayTypeEnumConverter.GetNameById(order.Id)*/
                                Number = order.Number,
                                PayType = EnumConverter.GetNameById<PayType>(order.PayType),
                                Seller = sellerQuery.First(),
                                DeliveryType = EnumConverter.GetNameById<DeliveryType>(order.DeliveryType),
                                IsComplaint = order.ComplaintDefinitionId != null,
                                Value = valueQuery,
                                CurrentOrderState = stateQuery.Result
                            };

                return query.ToList();
            }
        }

        public async Task SaveOrder(OrderDTO order, List<CartProductDTO> products, decimal amountToMinusBalance, VoucherDTO voucher = null)
        {
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    dbContext.Database.Log = (e) => Debug.WriteLine(e);
                    var orderDAL = new Orders
                    {
                        Number = ConfigurationService.GenerateElementNumber(),
                        CustomerId = order.CustomerId,
                        PayType = byte.Parse(order.PayType),
                        DeliveryType = byte.Parse(order.DeliveryType),
                        PickupId = order.PickupId,
                        SellerId = order.SellerId,
                        OrderDate = DateTime.Now,
                        ComplaintDefinitionId = null
                    };
                    dbContext.Orders.Add(orderDAL);
                    /* Zapisujemy zamówienie */
                    await dbContext.SaveChangesAsync(); //deadlock -> nie powoduje commitowania transakcji i ordercart nie moze odczytac orderId
                    /* Zapisujemy subelementy */
                    await ProductService.SaveOrderProductsForCart(products, orderDAL.Id);
                    /* Nadawanie statusu */
                    dbContext.OrderStates.Add(new OrderStates
                    {
                        OrderId = orderDAL.Id,
                        Date = DateTime.Now,
                        State = 0
                    });

                    /*Rozliczamy pieniadze */
                    if (voucher != null)
                    {
                        Vouchers originalVoucher = await dbContext.Vouchers.FirstAsync(p => p.Id == voucher.Id);
                        originalVoucher.Value = voucher.Value;
                    }

                    var customer = await CustomerService.GetCurrentCustomer();
                    customer.Balance -= amountToMinusBalance;
                    dbContext.Entry(customer).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();

                    order.Id = orderDAL.Id;
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

        public async Task<List<OrderDTO>> GetOrdersFromCompany(int sellerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var seller = await dbContext.Sellers.FindAsync(sellerId);

                var orderQuery = from order in dbContext.Orders
                                 let valueQuery = (from cart in dbContext.OrderCarts
                                                   from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                                   where cart.OrderId == order.Id
                                                   select cart.Amount * prodcut.PriceBrutto).Sum()
                                 let stateQuery = (from orderState in dbContext.OrderStates
                                                   where orderState.OrderId == order.Id
                                                   orderby orderState.Date descending
                                                   select orderState.State).FirstOrDefault() /* najpozniejszy status */
                                 where order.SellerId == sellerId
                                 select new { Order = order, Value = valueQuery, State = stateQuery };

                var ordersDAL = await orderQuery.AsNoTracking().ToListAsync();

                /* To DTO */
                var ordersDTO = ordersDAL.Select(p => new OrderDTO
                {
                    Id = p.Order.Id,
                    Number = p.Order.Number,
                    PayType = EnumConverter.GetNameById<PayType>(p.Order.PayType),
                    Seller = seller.Name,
                    DeliveryType = EnumConverter.GetNameById<DeliveryType>(p.Order.DeliveryType),
                    IsComplaint = p.Order.ComplaintDefinitionId != null,
                    Value = p.Value,
                    CurrentOrderState = p.State
                });
                return ordersDTO.ToList(); 
            }
        }
    }
}
