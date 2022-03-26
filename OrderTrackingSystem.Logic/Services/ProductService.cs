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
                                               select seller.Name).FirstOrDefault()
                            select new ProductDTO
                            {
                                Nazwa = product.Name,
                                Netto = product.PriceNetto.ToString(),
                                Rabat = product.Discount.ToString(),
                                VAT = product.VAT.ToString(),
                                Kategoria = product.Category.ToString(),
                                Sprzedawca = sellerQuery
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
    }
}
