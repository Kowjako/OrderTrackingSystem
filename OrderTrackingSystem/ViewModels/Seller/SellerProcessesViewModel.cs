using Microsoft.Win32;
using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class SellerProcessesViewModel : BaseViewModel, ISellerProcessesViewModel
    {
        #region Private members

        private string _sqlProcessScript = string.Empty;

        #endregion

        #region Bindable properties

        public List<ProcessDTO> SellerProcesses { get; set; } = new List<ProcessDTO>();
        public ProcessDTO NewSellerProcess { get; set; } = new ProcessDTO();

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

        public override async Task SetInitializeProperties()
        {
            SellerProcesses = await ConfigurationService.GetAutoProcesses();
            OnPropertyChanged(nameof(SellerProcesses));
        }

        #endregion

        #region Commands

        private RelayCommand _runProcesses;
        public RelayCommand RunProcesses =>
            _runProcesses ??= new RelayCommand(async obj =>
            {
                foreach(var process in SellerProcesses.Where(p => p.IsSelectedToRun))
                {
                    await ProcessRunner.RunProcedure(process.StoredProcedureFunction);
                }
            }, 
            (obj) => SellerProcesses.Any(p => p.IsSelectedToRun));

        private RelayCommand _selectAll;
        public RelayCommand SelectAll =>
            _selectAll ??= new RelayCommand(obj =>
            {
                SellerProcesses.ForEach(p => p.IsSelectedToRun = true);
                SellerProcesses = SellerProcesses.Select(p => p).ToList();
                OnPropertyChanged(nameof(SellerProcesses));
            }, (e) => SellerProcesses.Any());


        private RelayCommand _approveProcess;
        public RelayCommand ApproveProcess =>
            _approveProcess ??= new RelayCommand(obj =>
            {
                if(!string.IsNullOrEmpty(_sqlProcessScript))
                {
                    ConfigurationService.AddNewSellerProcess(NewSellerProcess, _sqlProcessScript);
                }
                else
                {
                    ShowWarning("Należy załadować plik z procedurą procesu");
                }
            });

        private RelayCommand _loadSQL;
        public RelayCommand LoadSQL =>
            _loadSQL ??= new RelayCommand(async obj =>
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = "SQL scripts|*.sql";
                if (ofd.ShowDialog() ?? false)
                {
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            _sqlProcessScript = await sr.ReadToEndAsync();
                        }
                    }
                }
            });
        #endregion
    }
}
