using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.CustomControls.Classes
{
    public class CustomerDictionary
    {
        public CustomerDTO Customer { get; set; }
        public bool IsChecked { get; set; }
    }

    public class CustomerSelectionViewModel : INotifyPropertyChanged
    {
        private readonly ICustomerService CustomerService;

        public bool HasData => _allCustomers != null && _allCustomers.Any();

        public List<CustomerDTO> SelectedCustomers => _allCustomers.Where(p => p.IsChecked).Select(p => p.Customer).ToList();

        private static List<CustomerDictionary> _allCustomers = new List<CustomerDictionary>();

        public event PropertyChangedEventHandler PropertyChanged;

        public List<CustomerDictionary> AllCustomers
        {
            get => _allCustomers;
            set => _allCustomers = value;
        }

        public CustomerSelectionViewModel()
        {
            CustomerService = new CustomerService();
        }

        public async void LoadData()
        {
            var customerList = await CustomerService.GetAllCustomers();
            customerList.ForEach(p => _allCustomers.Add(new CustomerDictionary { Customer = p, IsChecked = false }));
            RaisePropertyChanged(nameof(AllCustomers));
        }

        public void RaisePropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

    }
}
