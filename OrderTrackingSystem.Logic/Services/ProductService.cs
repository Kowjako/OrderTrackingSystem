using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class ProductService : IService<ProductService>
    {
        private CustomerService CustomerService => new CustomerService();

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
                                SellerId = sellerQuery.Id,
                                SubCategoryId = product.SubCateogryId.Value
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
                    var sellCart = new SellCarts
                    {
                        Amount = byte.Parse(product.Amount),
                        ProductId = product.Id,
                        SellId = sellId
                    };
                    dbContext.Entry(sellCart).State = EntityState.Added;
                    dbContext.SellCarts.Add(sellCart);
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<VoucherDTO>> GetVouchersForCurrentCustomer()
        {
            var customer = await CustomerService.GetCurrentCustomer();
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from voucher in dbContext.Vouchers
                            where voucher.CustomerId == customer.Id
                            select new VoucherDTO
                            {
                                Id = voucher.Id,
                                Number = voucher.Number,
                                Value = voucher.Value,
                                RemainValue = voucher.RemainValue,
                                ExpireDate = voucher.ExpireDate
                            };
                return await query.ToListAsync();
            }
        }

        public async Task<List<CategoryDTO>> GetProductCategories()
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from category in dbContext.ProductCategories
                            orderby category.ParentCategoryId
                            select category;
                var categoryList = await query.ToListAsync();
                List<CategoryDTO> categoriesOutput = new List<CategoryDTO>();
                /* Realizacja wzorca Composite */
                foreach(var category in categoryList)
                {
                    if(!category.ParentCategoryId.HasValue)
                    {
                        categoriesOutput.Add(new CategoryDTO()
                        {
                            Id = category.Id,
                            Title = category.Title,
                            Children = new List<CategoryDTO>()
                        });
                    }
                    else
                    {
                        categoriesOutput.First(p => p.Id == category.ParentCategoryId).Children.Add(new CategoryDTO()
                        {
                            Id = category.Id,
                            Title = category.Title,
                            Children = new List<CategoryDTO>()
                        });
                    }
                }
                return categoriesOutput;
            }
        }
    }
}
