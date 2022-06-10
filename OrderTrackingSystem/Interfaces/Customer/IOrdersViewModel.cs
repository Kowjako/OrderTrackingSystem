using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface IOrdersViewModel : INotifyableViewModel
    {
        PickupDTO SelectedPickup { get; set; }
        VoucherDTO SelectedVoucher { get; set; }
        ProductDTO SelectedProduct { get; set; }
        CustomerDTO CurrentCustomer { get; }
        List<PickupDTO> PickupsList { get; set; }
        OrderDTO CurrentOrder { get; set; }
        List<ProductDTO> AllProductsList { get; set; }
        List<ProductDTO> ProductsList { get; set; }
        List<VoucherDTO> VouchersList { get; set; }
        BindingList<CartProductDTO> ProductsInCart { get; set; }

        Visibility VouchersVisibility { get; }
        decimal TotalPriceNetto { get; set; }
        decimal VAT { get; }
        decimal TotalPriceBrutto { get; }
        decimal DeliveryCost { get; }
        decimal FullPrice { get; }
        int SelectedDeliveryType { get; set; }

        Task SetInitializeProperties();

        RelayCommand AddToCart { get; }
        RelayCommand FindSeller { get; }
        RelayCommand MinusAmount { get; }
        RelayCommand PlusAmount { get; }
        RelayCommand AcceptOrder { get; }
        RelayCommand ClearCart { get; }
    }
}
