using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using System.Collections.Generic;
using System.Linq;

namespace OrderTrackingSystem.Tests.HelpersTests
{
    class HelperDataProvider
    {
        public static IEnumerable<object[]> GetStatesWithResult()
        {
            yield return new object[] { new StateA(), new[] { OrderState.GetFromSeller } };
            yield return new object[] { new StateB(), new[] { OrderState.GetByLocal } };
            yield return new object[] { new StateC(), new[] { OrderState.SentFromLocal } };
            yield return new object[] { new StateD(), new[] { OrderState.ToDelivery, OrderState.GetByLocal } };
            yield return new object[] { new StateE(), new[] { OrderState.ReadyToPickup } };
            yield return new object[] { new StateF(), new[] { OrderState.ComplaintSet } };
            yield return new object[] { new StateG(), new[] { OrderState.ComplaintResolved, OrderState.ReturnToSeller } };
            yield return new object[] { new StateH(), new[] { OrderState.ReturnToSeller } };
            yield return new object[] { new StateI(), Enumerable.Empty<OrderState>() };
            yield return new object[] { new StateJ(), Enumerable.Empty<OrderState>() };
        }

        public static IEnumerable<object[]> GetICompositePattern()
        {
            yield return new object[] { new CategoryDTO() {
                Name = "1",
                Children = new List<CategoryDTO>()
                {
                    new CategoryDTO() {Name = "1.1", Children = new List<CategoryDTO>()
                    {
                        new CategoryDTO() {Name = "1.1.1"},
                        new CategoryDTO() {Name = "1.1.2"},
                        new CategoryDTO() {Name = "1.1.3"}
                    } },
                    new CategoryDTO() {Name = "1.2"}
                }
            } };
        }

        public static IEnumerable<CategoryDTO> GetCompositeList()
        {
            //Trzy glowne grupy
            yield return new CategoryDTO() { Id = 1, Name = "1", ParentId = null, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 2, Name = "2", ParentId = null, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 3, Name = "3", ParentId = null, Children = new List<CategoryDTO>() };

            //Potomki grupy 1
            yield return new CategoryDTO() { Id = 4, Name = "1.1", ParentId = 1, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 5, Name = "1.2", ParentId = 1, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 6, Name = "1.3", ParentId = 1, Children = new List<CategoryDTO>() };

            //Potomki grupy 2
            yield return new CategoryDTO() { Id = 7, Name = "2.1", ParentId = 2, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 8, Name = "2.2", ParentId = 2, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 9, Name = "2.3", ParentId = 2, Children = new List<CategoryDTO>() };

            //Potomki grupy 3
            yield return new CategoryDTO() { Id = 10, Name = "3.1", ParentId = 3, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 11, Name = "3.2", ParentId = 3, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 12, Name = "3.3", ParentId = 3, Children = new List<CategoryDTO>() };

            //Potomki grupy 1.1
            yield return new CategoryDTO() { Id = 13, Name = "1.1.1", ParentId = 4, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 14, Name = "1.1.2", ParentId = 4, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 15, Name = "1.1.3", ParentId = 4, Children = new List<CategoryDTO>() };

            //Potomki grupy 3.2
            yield return new CategoryDTO() { Id = 16, Name = "3.2.1", ParentId = 11, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 17, Name = "3.2.2", ParentId = 11, Children = new List<CategoryDTO>() };
            yield return new CategoryDTO() { Id = 18, Name = "3.2.3", ParentId = 11, Children = new List<CategoryDTO>() };
        }

    }
}
