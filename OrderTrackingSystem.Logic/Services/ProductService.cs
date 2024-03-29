﻿using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class ProductService : CRUDManager, IProductService
    {
        private ICustomerService CustomerService;
        private IConfigurationService ConfigurationService;

        public ProductService(IConfigurationService confService)
        {
            ConfigurationService = confService;
            CustomerService = new CustomerService(confService);
        }

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
                                Name = product.Name,
                                PriceNetto = product.PriceNetto,
                                Discount = product.Discount,
                                TAX = product.VAT.ToString(),
                                Seller = sellerQuery.Name,
                                SellerId = sellerQuery.Id,
                                Category = productCategories.Title,
                                CategoryId = product.Category,
                                ImageData = product.ImageData
                            };

                return await query.ToListAsync();
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
                        Amount = decimal.ToInt16(product.Amount),
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
                        Amount = decimal.ToByte(product.Amount),
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
                            where voucher.CustomerId == customer.Id && 
                                  voucher.Value != 0 &&
                                  voucher.ExpireDate >= DateTime.Now /* Zwracamy tylko bony mające kwotę */
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

        public async Task SaveNewProduct(Products product, byte[] imageData)
        {
            product.PriceBrutto = Math.Round(product.PriceNetto + product.PriceNetto * product.VAT / 100.0m, 2, MidpointRounding.ToEven);
            product.ImageData = imageData;
            await AddEntity(product);
        }
         
        public async Task GenerateVouchersForCustomer(VoucherDTO voucher, params int[] customerIds)
        {
            if(!customerIds.Any())
            {
                throw new InvalidOperationException("Lista klientów nie może być pusta");
            }

            using (var dbContext = new OrderTrackingSystemEntities())
            {
                customerIds.ToList().ForEach(c =>
                {
                    var voucherDAL = new Vouchers()
                    {
                        Number = ConfigurationService.GenerateElementNumber(),
                        Value = voucher.Value,
                        RemainValue = voucher.Value,
                        ExpireDate = voucher.ExpireDate,
                        CustomerId = c
                    };
                    dbContext.Vouchers.Add(voucherDAL);
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
