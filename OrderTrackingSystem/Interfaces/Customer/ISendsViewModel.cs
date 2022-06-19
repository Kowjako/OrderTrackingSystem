using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface ISendsViewModel : INotifyableViewModel
    {
        CategoryDTO SelectedSubCategory { get; set; }
        Customers CurrentSeller { get; }
        CustomerDTO CurrentReceiver { get; }
        List<ProductDTO> AllProductsList { get; set; }
        List<ProductDTO> ProductsList { get; set; }
        List<CategoryDTO> CategoriesList { get; set; }
        ProductDTO SelectedProduct { get; set; }
        BindingList<CartProductDTO> ProductsInCart { get; set; }

        Task SetInitializeProperties();

        RelayCommand MinusAmount { get; }
        RelayCommand PlusAmount { get; }
        RelayCommand FindReceiver { get; }
        RelayCommand AddToCart { get; }
        RelayCommand FilterCommand { get; }
        RelayCommand ClearCart { get; }
        RelayCommand AcceptSell { get; }
    }
}
