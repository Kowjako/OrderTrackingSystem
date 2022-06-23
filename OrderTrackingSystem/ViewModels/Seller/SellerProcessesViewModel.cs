using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class SellerProcessesViewModel : ISellerProcessesViewModel, INotifyPropertyChanged
    {
        #region Bindable properties

        public List<ProcessDTO> SellerProcesses { get; set; } = new List<ProcessDTO>();

        #endregion

        #region Ctor

        public SellerProcessesViewModel()
        {
            SellerProcesses.Add(new ProcessDTO("Processes.CheckOrdersTermin")
            {
                Name = "Sprawdzanie terminowosci dostarczenia zamowien",
                Description = "Gdy zostało mniej niż 2 dni do dostarczenia zamówienia jest wysyłana wiadomość do klienta, gdy zamówienie zostalo przeterminowane - zamówienie jest usuwane a kwota zwracana kleintowi",
                LastProcessDate = DateTime.Now
            });
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {

        }

        #endregion


        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void OnManyPropertyChanged(IEnumerable<string> props)
        {
            foreach (var prop in props)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion

    }
}
