using OrderTrackingSystem.Tests.ClassFixtures;
using OrderTrackingSystem.Tests.DatabaseFixture;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using Xunit.Extensions.Ordering;
using System.Linq;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System;
using OrderTrackingSystem.Logic.EnumMappers;
using Moq;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System.Threading.Tasks;
using OrderTrackingSystem.Logic.Services;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class ComplaintsTests : IClassFixture<ComplaintTestFixture>
    {
        ComplaintTestFixture context;

        public ComplaintsTests(ComplaintTestFixture fixture)
        {
            context = fixture;
        }

        [Fact, Order(1)]
        public async void ComplTest_ProvidedData_ShouldSaveFolder()
        {
            //arrange
            var folder = "Folder 1";

            //act
            await context.ComplaintService.AddNewFolder(folder, null);
            var list = await context.ComplaintService.GetComplaintFolders();

            //assert
            Assert.Contains(folder, list.Select(p => p.Name));
        }

        [Fact, Order(2)]
        public async void ComplTest_AddedFolder_ShouldReturnAllFolders()
        {
            //arrange
            var folder = "Folder 1";
            await context.ComplaintService.AddNewFolder(folder, null);
            var parent = (await context.ComplaintService.GetComplaintFolders()).First();
            await context.ComplaintService.AddNewFolder("Folder 1.1", parent);
            await context.ComplaintService.AddNewFolder("Folder 1.2", parent);

            var expectedNames = new[] { "Folder 1", "Folder 1.1", "Folder 1.2" };

            //act
            var list = await context.ComplaintService.GetComplaintFoldersWithoutComposing();

            //assert
            Assert.All(expectedNames, expectedName => Assert.Contains(expectedName, list.Select(p => p.Name)));
        }

        [Fact, Order(3)]
        public async void ComplTest_ProvidedData_ShouldComposeFolders()
        {
            //arrange
            var folder = "Folder 2";
            await context.ComplaintService.AddNewFolder(folder, null);

            var parent = (await context.ComplaintService.GetComplaintFolders()).Where(p => p.Name.Equals("Folder 2")).First();
            await context.ComplaintService.AddNewFolder("Folder 2.1", parent);
            await context.ComplaintService.AddNewFolder("Folder 2.2", parent);

            //act
            var list = await context.ComplaintService.GetComplaintFolders();
            var folder1 = list.First(p => p.Name.Equals(folder));

            //assert
            Assert.Contains(folder, list.Select(p => p.Name));
            Assert.Equal(2, folder1.Children.Count);
        }

        [Fact, Order(4)]
        public async void ComplTest_ProvidedTemplate_ShouldSaveTemplate()
        {
            //arrange
            var complaintDefinition = OF.ObjectFactory.CreateComplaintDefinition();
            var folder = "Folder XXL";
            await context.ComplaintService.AddNewFolder(folder, null);

            var folderId = (await context.ComplaintService.GetComplaintFolders()).Where(p => p.Name.Equals(folder))
                                                                                 .Select(p => p.Id)
                                                                                 .First();
            //act
            await context.ComplaintService.SaveComplaintTemplate(complaintDefinition, folderId);

            //assert

            Assert.True(complaintDefinition.Id > 0);
        }

        [Fact, Order(5)]
        public async void ComplTest_ProvidedData_ShouldRegisterComplaint()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDb();
            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };

            //mock
            var mock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(customer.Id));
            context.OrderService = new OrderService(new CustomerService(mock));
            await context.OrderService.SaveOrder(order, elemList, 100.0m);

            var complaintDefinitionId = await context.EntitiesGenerator.AddNewComplaintDefinitionToDb();

            //act
            await context.ComplaintService.RegisterNewComplaint(complaintDefinitionId, order.Id);
            var complaints = await context.ComplaintService.GetComplaintsForCustomer(customer.Id);

            //assert
            Assert.True(complaints.Count == 1);
        }

        [Fact, Order(6)]
        public async void ComplTest_UpdateComplaintState_CheckMailAndState()
        {
            //arrange
            (var complaintId, var customerId, var sellerId) = await context.EntitiesGenerator.AddNewComplaintToDb();
            var complaint = (await context.ComplaintService.GetComplaintsForCustomer(customerId)).First(p => p.Id == complaintId);

            var complaintX = OF.ObjectFactory.CreateComplaintStateByDTO(complaint);
            complaintX.SolutionDate = DateTime.Now;
            complaintX.State = (byte)ComplaintState.SellerDecision;
            var mailCountFirst = await context.MailService.GetReceivedMailsForCustomer(customerId);

            //act
            await context.ComplaintService.UpdateComplaintState(complaintX, sellerId);
            var newComplaint = (await context.ComplaintService.GetComplaintsForCustomer(customerId)).First(p => p.Id == complaintId);
            var mailCountSecond = await context.MailService.GetReceivedMailsForCustomer(customerId);
           
            //assert
            Assert.NotNull(newComplaint.SolutionDate); //data ustawiona
            Assert.True(newComplaint.StateId == (byte)ComplaintState.SellerDecision); //status zmieniony
            Assert.Equal(1, mailCountSecond.Count - mailCountFirst.Count); //mail wyslany
        }

        [Fact, Order(7)]
        public async void ComplTest_CloseComplaint_CheckCloseDate()
        {
            //arrange
            (var complaintId, var customerId, var sellerId) = await context.EntitiesGenerator.AddNewComplaintToDb();
            var complaint = (await context.ComplaintService.GetComplaintsForCustomer(customerId)).First(p => p.Id == complaintId);
            var complaintX = OF.ObjectFactory.CreateComplaintStateByDTO(complaint);

            //act
            await context.ComplaintService.CloseComplaint(complaintX);
            var newComplaint = (await context.ComplaintService.GetComplaintsForCustomer(customerId)).First(p => p.Id == complaintId);

            //assert
            Assert.NotNull(newComplaint.EndDate);
            Assert.True(newComplaint.StateId == (byte)ComplaintState.ComplaintSolved);
        }

        [Fact, Order(8)]
        public async void ComplTest_CustomerMadeComplaint_SellerShouldReceive()
        {
            //arrange
            (var complaintId, var customerId, var sellerId) = await context.EntitiesGenerator.AddNewComplaintToDb();

            //act
            var list = await context.ComplaintService.GetComplaintsForSeller(sellerId);

            //assert
            Assert.Contains(complaintId, list.Select(p => p.Id));
        }

        [Fact, Order(9)]
        public async void ComplTest_DeletingWithAncestor_ShouldMoveToParent()
        {
            //arrange
            var folders = await context.EntitiesGenerator.AddComplaintFoldersTreeToDb("1");

            // 121
            // -- 12.11
            // -- 12.21
            //       --12.2.11
            //       --12.2.21
            //       --12.2.31

            //act
            await context.ComplaintService.DeleteAndMoveToAncestor(folders.First(p => p.Name.Equals("Folder 121"))
                                                                          .Children
                                                                          .First(p => p.Name.Equals("Folder 12.21")));
            var list = (await context.ComplaintService.GetComplaintFolders()).First(p => p.Name.Equals("Folder 121"));

            //assert
            Assert.Equal(4, list.Children.Count);           
        }

        [Fact, Order(10)]
        public async void ComplTest_DeletingWithMoving_ShouldRemoveAll()
        {
            //arrange
            var folders = await context.EntitiesGenerator.AddComplaintFoldersTreeToDb("2");

            // 122
            // -- 12.12
            // -- 12.22
            //       --12.2.12
            //       --12.2.22
            //       --12.2.32

            //act
            await context.ComplaintService.DeleteWithAncestor(folders.First(p => p.Name.Equals("Folder 122"))
                                                                     .Children
                                                                     .First(p => p.Name.Equals("Folder 12.22")));
            var list = (await context.ComplaintService.GetComplaintFolders()).First(p => p.Name.Equals("Folder 122"));

            //assert
            Assert.Single(list.Children);
        }
    }
}
