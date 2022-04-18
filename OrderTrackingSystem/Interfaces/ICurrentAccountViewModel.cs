using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Interfaces
{
    public interface ICurrentAccountViewModel
    {
        Customers CurrentCustomer { get; set; }
        List<LocalizationDTO> Localization { get; set; }
        List<OrderDTO> Orders { get; set; }
        List<SellDTO> Sells { get; set; }

        Task SetInitializeProperties();

        RelayCommand SaveCommand { get; }
    }
}
