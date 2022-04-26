using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class StartupViewModel
    {
        #region Services

        private readonly ConfigurationService ConfigurationService;

        #endregion

        #region Bindable properties

        public string Login { get; set; }
        public string Password { get; set; }

        #endregion

        #region Ctor

        public StartupViewModel()
        {
            ConfigurationService = new ConfigurationService();
        }

        #endregion

        #region Commands

        private RelayCommand _login;
        public RelayCommand LoginCmd =>
            _login ?? (_login = new RelayCommand(async obj =>
            {
                try
                {
                   
                }
                catch (Exception)
                {

                }
            }));

        private RelayCommand _createNewAccount;
        public RelayCommand CreateNewAccount =>
            _createNewAccount ?? (_createNewAccount = new RelayCommand(async obj =>
            {
                try
                {

                }
                catch (Exception)
                {

                }
            }));

        #endregion
    }
}
