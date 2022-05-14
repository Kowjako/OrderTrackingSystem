using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTrackingSystem.Logic.Services
{
    public class SellService : ISellService
    {
        private ProductService ProductService => new ProductService();
        private IConfigurationService ConfigurationService = new ConfigurationService();

        public async Task<List<SellDTO>> GetSellsForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.Where(c => c.Id == customerId).FirstAsync();

                IList<Sells> Sells = await dbContext.Sells.Where(p => p.SellerId == customer.Id).ToListAsync();

                var query = from sells in Sells
                            let valueQuery =  (from cart in dbContext.SellCarts
                                              from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                              where cart.SellId == sells.Id
                                              select cart.Amount * prodcut.PriceBrutto).Sum()
                            let receiverQuery = (from receiver in dbContext.Customers
                                                where receiver.Id == sells.CustomerId
                                                select receiver.Name + " " + receiver.Surname).ToList()
                            select new SellDTO
                            {
                                Numer = sells.Number,
                                Data = sells.SellingDate.ToShortDateString(),
                                Dni = sells.PickupDays.ToString(),
                                Odbiorca = receiverQuery.First(),
                                Kwota = string.Format("{0:0.00 zł}", valueQuery)
                            };

                return query.ToList();
            }
        }

        public async Task SaveSell(SellDTO order, List<CartProductDTO> products)
        {
            var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    var sellDAL = new Sells
                    {
                        Number = ConfigurationService.GenerateElementNumber(),
                        SellingDate = DateTime.Now,
                        CustomerId = order.CustomerId,
                        SellerId = order.SellerId,
                        PickupDays = order.PickupDays == 0 ? 5 : order.PickupDays.Value
                    };
                    dbContext.Sells.Add(sellDAL);
                    /* Zapisujemy wysyłkę */
                    await dbContext.SaveChangesAsync();
                    /* Zapisujemy subelementy */
                    await ProductService.SaveSellProductsForCart(products, sellDAL.Id);
                    /* Ustawiamy numer po dodaniu do bazy */
                    order.Numer = sellDAL.Number;
                }
                transactionScope.Complete();
            }

        }
    }
}
