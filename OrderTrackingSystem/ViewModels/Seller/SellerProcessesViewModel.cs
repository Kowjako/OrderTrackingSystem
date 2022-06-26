using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
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

        #region Services

        private readonly IConfigurationService ConfigurationService;

        #endregion

        #region Ctor

        public SellerProcessesViewModel()
        {
            ConfigurationService = new ConfigurationService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            SellerProcesses = await ConfigurationService.GetAutoProcesses();
            OnPropertyChanged(nameof(SellerProcesses));
        }

        #endregion

        #region Commands

        private RelayCommand _runProcesses;
        public RelayCommand RunProcesses =>
            _runProcesses ?? (_runProcesses = new RelayCommand(async obj =>
            {
                foreach(var process in SellerProcesses.Where(p => p.IsSelectedToRun))
                {
                    await ProcessRunner.RunProcedure(process.StoredProcedureFunction);
                }
            }, 
            (obj) => SellerProcesses.Any(p => p.IsSelectedToRun)));

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
