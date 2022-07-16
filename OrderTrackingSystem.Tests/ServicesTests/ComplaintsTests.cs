using OrderTrackingSystem.Tests.ClassFixtures;
using OrderTrackingSystem.Tests.DatabaseFixture;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using Xunit.Extensions.Ordering;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class ComplaintsTests : IClassFixture<ComplaintTestFixture>
    {
        DBFixture db;
        ComplaintTestFixture context;

        public ComplaintsTests(DBFixture db, ComplaintTestFixture fixture)
        {
            this.db = db;
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
        public async void ComplTest_ProvidedData_ShouldDeleteWithAncestor()
        {

        }
    }
}
