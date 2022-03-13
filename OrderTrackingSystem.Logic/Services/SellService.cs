﻿using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class SellService : IService<SellService>
    {
        public async Task<List<SellDTO>> GetSellsForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.Where(c => c.Id == customerId).FirstAsync();
                await dbContext.Entry(customer).Collection(nameof(customer.Sells)).LoadAsync();

                var query = from sells in customer.Sells
                            let valueQuery = (from cart in dbContext.SellCarts
                                              from prodcut in dbContext.Products.Where(p => p.Id == cart.ProductId).DefaultIfEmpty()
                                              where cart.SellId == sells.Id
                                              select cart.Amount * prodcut.PriceBrutto).Sum()
                            let receiverQuery = from receiver in dbContext.Customers
                                                where receiver.Id == sells.CustomerId
                                                select receiver.Name + " " + receiver.Surname
                            select new SellDTO
                            {
                                Numer = sells.Number,
                                Data = sells.SellingDate.ToShortDateString(),
                                Dni = sells.PickupDays.ToString(),
                                Odbiorca = receiverQuery.FirstAsync().Result,
                                Kwota = string.Format("{0:0.00 zł}", valueQuery)
                            };

                return query.ToList();
            }
        }
    }
}
