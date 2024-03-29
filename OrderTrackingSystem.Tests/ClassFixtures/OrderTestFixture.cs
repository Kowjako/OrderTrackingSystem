﻿using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class OrderTestFixture : IDisposable
    {
        public EntitiesGenerator EntitiesGenerator;
        public IOrderService OrderService;
        public ICustomerService CustomerService;
        public IProductService ProductService;

        public OrderTestFixture()
        {
            EntitiesGenerator = new EntitiesGenerator();
            CustomerService = new CustomerService(new ConfigurationService());
            OrderService = new OrderService(CustomerService);
            ProductService = new ProductService(new ConfigurationService());
        }

        public void Dispose() { }
    }
}
