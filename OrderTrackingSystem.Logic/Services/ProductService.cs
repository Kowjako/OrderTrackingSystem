using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class ProductService : IService<ProductService>
    {
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from product in dbContext.Products
                            let sellerQuery = (from seller in dbContext.Sellers
                                               where seller.Id == product.SellerId
                                               select seller).FirstOrDefault()
                            select new ProductDTO
                            {
                                Id = product.Id,
                                Nazwa = product.Name,
                                Netto = product.PriceNetto.ToString(),
                                Rabat = product.Discount.ToString(),
                                VAT = product.VAT.ToString(),
                                Kategoria = product.Category.ToString(),
                                Sprzedawca = sellerQuery.Name,
                                SellerId = sellerQuery.Id
                            };

                var productsList = await query.ToListAsync();

                /* Formatowanie przed zwracaniem listy */
                productsList.ForEach(p =>
                {
                    p.Netto = string.Format("{0:0.00} zł", p.Netto);
                    p.Rabat = string.Format("{0} %", p.Rabat);
                    p.VAT = string.Format("{0} %", p.VAT);
                    p.Kategoria = EnumConverter.GetNameById<ProductType>(byte.Parse(p.Kategoria));
                });
                return productsList;
            }
        }

        public async Task SaveOrderProductsForCart(List<CartProductDTO> products, int orderId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                foreach (var product in products)
                {
                    var orderCart = new OrderCarts
                    {
                        Amount = short.Parse(product.Amount),
                        ProductId = product.Id,
                        OrderId = orderId
                    };
                    dbContext.Entry(orderCart).State = EntityState.Added;
                    dbContext.OrderCarts.Add(orderCart);
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveSellProductsForCart(List<CartProductDTO> products, int sellId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                foreach (var product in products)
                {
                    dbContext.SellCarts.Add(new SellCarts
                    {
                        Amount = byte.Parse(product.Amount),
                        ProductId = product.Id,
                        SellId = sellId
                    });
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
