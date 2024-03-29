﻿using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System.Collections.Generic;

namespace OrderTrackingSystem.Logic.Services
{
    public class CustomerService : CRUDManager, ICustomerService
    {
        private IConfigurationService _configurationService;

        public CustomerService(IConfigurationService service)
        {
            _configurationService = service;
        }

        public async Task<Customers> GetCurrentCustomer()
        {
            var sessionId = await _configurationService.GetCurrentSessionId();
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                return await dbContext.Customers.FindAsync(sessionId);
            }
        }

        public async Task<Sellers> GetCurrentSeller()
        {
            var sessionId = await _configurationService.GetCurrentSessionId();
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                return await dbContext.Sellers.FindAsync(sessionId);
            }
        }

        public async Task UpdateCustomer(Customers customer)
        {
            await base.UpdateEntity(customer);
        }

        public async Task UpdateSeller(Sellers seller)
        {
            await base.UpdateEntity(seller);
        }

        public async Task<CustomerDTO> GetCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                IQueryable<CustomerDTO> query = null;
                query = from customer in dbContext.Customers
                        join loc in dbContext.Localizations  /* INNER JOIN */
                        on customer.LocalizationId equals loc.Id
                        where customer.Id == customerId

                        select new CustomerDTO
                        {
                            Id = customer.Id,
                            Name = customer.Name + " " + customer.Surname,
                            Address = loc.Street + " " +
                                    loc.House + ", " +
                                    loc.Flat,
                            Email = customer.Email,
                            CityWithCode = loc.City + ", " +
                                        loc.ZipCode,
                            Number = customer.Number,
                            Balance = customer.Balance
                        };
                return await query.FirstOrDefaultAsync();
            }
        }
        
        public async Task<CustomerDTO> GetCustomerByName(string name)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                name = name.Replace(" ", string.Empty).ToLower();
                IQueryable<CustomerDTO> query = null;
                query = from customer in dbContext.Customers
                        join loc in dbContext.Localizations
                        on customer.LocalizationId equals loc.Id
                        where (customer.Name + customer.Surname).ToLower().Equals(name)

                        select new CustomerDTO
                        {
                            Id = customer.Id,
                            Name = customer.Name + " " + customer.Surname,
                            Address = loc.Street + " " +
                                    loc.House + ", " +
                                    loc.Flat,
                            Email = customer.Email,
                            CityWithCode = loc.City + ", " +
                                        loc.ZipCode,
                            Number = customer.Number
                        };
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<CustomerDTO> GetSellerByName(string name)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                name = name.Replace(" ", string.Empty).ToLower();
                IQueryable<CustomerDTO> query = null;
                query = from seller in dbContext.Sellers
                        join loc in dbContext.Localizations
                        on seller.LocalizationId equals loc.Id
                        where seller.Name.ToLower().Equals(name)
                        select new CustomerDTO
                        {
                            Id = seller.Id,
                            Name = seller.Name,
                            Address = loc.Street + " " +
                                    loc.House + ", " +
                                    loc.Flat,
                            Email = seller.Email,
                            CityWithCode = loc.City + ", " +
                                        loc.ZipCode,
                            Number = seller.Number
                        };
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<CustomerDTO> GetSeller(int sellerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                IQueryable<CustomerDTO> query = null;
                query = from seller in dbContext.Sellers
                        join loc in dbContext.Localizations
                        on seller.LocalizationId equals loc.Id
                        where seller.Id == sellerId

                        select new CustomerDTO
                        {
                            Name = seller.Name,
                            Address = loc.Street + " " +
                                    loc.House + ", " +
                                    loc.Flat,
                            Email = seller.Email,
                            CityWithCode = loc.City + ", " +
                                        loc.ZipCode,
                            Number = seller.Number
                        };
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task AddNewCustomer(Customers customer, int localizationId, (string login, string password) credentials)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                customer.LocalizationId = localizationId;
                await base.AddEntity(customer);

                var encryptedPassword = Cryptography.EncryptWithRSA(credentials.password);

                var user = new Users()
                {
                    AccountId = customer.Id,
                    Login = credentials.login,
                    Password = encryptedPassword,
                    AccountType = true
                };

                await base.AddEntity(user);
            }
        }

        public async Task AddNewSeller(Sellers seller, int localizationId, (string login, string password) credentials)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                seller.LocalizationId = localizationId;
                await base.AddEntity(seller);

                var encryptedPassword = Cryptography.EncryptWithRSA(credentials.password);

                var user = new Users()
                {
                    AccountId = seller.Id,
                    Login = credentials.login,
                    Password = encryptedPassword,
                    AccountType = false
                };

                await base.AddEntity(user);
            }
        }

        public async Task<CustomerDTO> GetCustomerByMail(string email)
        {
            var customer = await base.GetEntity<Customers>(p => p.Email.Equals(email));
            return new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
            };
        }

        public async Task<List<CustomerDTO>> GetAllCustomers()
        {
            var dalEntities = await base.GetAllEntities<Customers>();
            return dalEntities.Select(p => new CustomerDTO()
            {
                Id = p.Id,
                Name = p.Name + " " + p.Surname,
                Email = p.Email,
            }).ToList();
        }
    }
}
