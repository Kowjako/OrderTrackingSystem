﻿using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class MailTestFixture : IDisposable
    {
        public EntitiesGenerator EntitiesGenerator;
        public IMailService MailService;
        public ICustomerService CustomerService;
        public IOrderService OrderService;

        public MailTestFixture()
        {
            EntitiesGenerator = new EntitiesGenerator();
            CustomerService = new CustomerService(new ConfigurationService());
            MailService = new MailService();
            OrderService = new OrderService(CustomerService);
        }

        public void Dispose() { }
    }
}
