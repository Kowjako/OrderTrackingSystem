using OrderTrackingSystem.Tests.ClassFixtures;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Tests.DatabaseFixture;
using System.Linq;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using OrderTrackingSystem.Logic.EnumMappers;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class MailServiceTests : IClassFixture<MailTestFixture>
    {
        MailTestFixture context;

        public MailServiceTests(MailTestFixture fixture)
        {
            context = fixture;
        }

        [Fact]
        public async void MailTest_ProvidedData_ShouldAddMail()
        {
            //arrange
            var mail = OF.ObjectFactory.CreateMail();
            var customer = await context.EntitiesGenerator.AddNewCustomerToDb();
            var seller = await context.EntitiesGenerator.AddNewSellerToDb();

            mail.SenderId = customer.Id; 
            mail.ReceiverId = seller.Id;

            //act
            await context.MailService.AddNewMail(mail);

            //assert
            Assert.True(mail.Id > 0);
        }

        [Fact]
        public async void MailTest_CustomerSend_ShouldGetSeller()
        {
            //arrange
            (var mail, int senderId, int receiverId) = await context.EntitiesGenerator.AddNewMailToDbCusToSeller();

            //act
            var mailsForCustomer = await context.MailService.GetSendMailsForCustomer(senderId);
            var mailsForSeller = await context.MailService.GetReceivedMailsForSeller(receiverId);

            //assert
            Assert.Contains(mail.Id, mailsForCustomer.Select(p => p.Id));
            Assert.Contains(mail.Id, mailsForSeller.Select(p => p.Id));
        }

        [Fact]
        public async void MailTest_SellerSend_ShoudGetCustomer()
        {
            //arrange
            (var mail, int senderId, int receiverId) = await context.EntitiesGenerator.AddNewMailToDbSellerToCus();

            //act
            var mailsForSeller = await context.MailService.GetSendMailsForSeller(senderId);
            var mailsForCustomer = await context.MailService.GetReceivedMailsForCustomer(receiverId);

            //assert
            Assert.Contains(mail.Id, mailsForCustomer.Select(p => p.Id));
            Assert.Contains(mail.Id, mailsForSeller.Select(p => p.Id));
        }

        [Fact]
        public async void MailTest_SendMailWithOrders_ShouldFetchOrders()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDb();
            var mail = OF.ObjectFactory.CreateMailDTO();
            mail.MailRelation = (int)MailDirectionType.CustomerToSeller;
            mail.SellerId = customer.Id;
            mail.ReceiverId = order.SellerId;
            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };
            await context.OrderService.SaveOrder(order, elemList, 100.0m);

            await context.MailService.SendMail(mail: mail, relatedOrders: new[] { order.Number });

            //act
            var mailList = await context.MailService.GetSendMailsForCustomer(order.CustomerId);

            //assert
            Assert.NotNull(mailList.FirstOrDefault());
            Assert.Contains(order.Number, mailList.FirstOrDefault().RelatedOrders);
        }
    }
}
