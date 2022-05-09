using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class ProductService : CRUDManager, IService<ProductService>
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
                            join productCategories in dbContext.ProductCategories
                            on product.Category equals productCategories.Id
                            select new ProductDTO
                            {
                                Id = product.Id,
                                Nazwa = product.Name,
                                Netto = product.PriceNetto.ToString(),
                                Rabat = product.Discount.ToString(),
                                VAT = product.VAT.ToString(),
                                Sprzedawca = sellerQuery.Name,
                                SellerId = sellerQuery.Id,
                                Kategoria = productCategories.Title,
                                CategoryId = product.Category
                            };

                var productsList = await query.ToListAsync();

                /* Formatowanie przed zwracaniem listy */
                productsList.ForEach(p =>
                {
                    p.Netto = string.Format("{0:0.00} zł", p.Netto);
                    p.Rabat = string.Format("{0} %", p.Rabat);
                    p.VAT = string.Format("{0} %", p.VAT);
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

                /* Zwracamy tylko bony mające kwotę */
                return await query.Where(p => p.Value != 0).ToListAsync();
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

                var categoryListDTO = categoryList.Select(p =>
                new CategoryDTO
                {
                    Id = p.Id,
                    Name = p.Title,
                    Children = new List<CategoryDTO>(),
                    ParentId = p.ParentCategoryId
                }).ToList();

                /* Rekurencyjne wypelnianie drzewa */
                RecursiveTreeFiller<CategoryDTO>.FillTreeRecursive(categoryListDTO);

                return categoryListDTO.Where(p => p.ParentId == null).ToList();
            }
        }

        public async Task<List<CategoryDTO>> GetProductSubCategories()
        {
            var parentsList = await GetProductCategories();
            parentsList.ForEach(p => p.Children.ForEach(x => x.Name = string.Format("{0} - {1}", p.Name, x.Name)));
            var outputList = Enumerable.Empty<CategoryDTO>();
            parentsList.ForEach(p => outputList = outputList.Union(p.Children));
            return outputList.ToList();
        }

        public async Task SaveNewProduct(Products product)
        {
            product.PriceBrutto = Math.Round(product.PriceNetto + product.PriceNetto * product.VAT / 100.0m, 2, MidpointRounding.ToEven);
            await AddEntity(product);
        }
    }
}
