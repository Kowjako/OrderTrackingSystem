using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class StartupViewModel
    {
        #region Services

        private readonly ConfigurationService ConfigurationService;
        private readonly CustomerService CustomerService;
        private readonly LocalizationService LocalizationService;

        #endregion

        #region Bindable properties

        public string Login { get; set; }
        public bool CreationForClient { get; set; }

        public Localizations Localization { get; set; } = new Localizations();
        public Customers NewCustomer { get; set; } = new Customers();
        public Sellers NewSeller { get; set; } = new Sellers();

        public string[] Credentials { get; set; } = new string[2];

        #endregion

        #region Ctor

        public StartupViewModel()
        {
            ConfigurationService = new ConfigurationService();
            CustomerService = new CustomerService();
            LocalizationService = new LocalizationService();
        }

        #endregion

        #region Commands

        private RelayCommand _login;
        public RelayCommand LoginCmd =>
            _login ?? (_login = new RelayCommand(async obj =>
            {
                try
                {
                    var passwordBox = obj as PasswordBox;
                    if(!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(passwordBox.Password))
                    {
                        (bool isSuccess, bool accType) = await ConfigurationService.MakeSessionForCredentials(Login, passwordBox.Password);
                        if(isSuccess)
                        {
                            var mainWindow = new MainWindow()
                            {
                                IsAppForClient = accType
                            };
                            mainWindow.Show();
                            Application.Current.Windows[0].Close(); /* zamykamy okno logowania */
                        }
                    }
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
                    bool isValidEntity = CreationForClient switch
                    {
                        true => ValidatorWrapper.ValidateWithResult(new CustomerValidator(), NewCustomer),
                        false => ValidatorWrapper.ValidateWithResult(new SellerValidator(), NewSeller)
                    };

                    var msg = ValidatorWrapper.ErrorMessage;
                    isValidEntity &= ValidatorWrapper.ValidateWithResult(new LocalizationValidatorDAL(), Localization);

                    if(isValidEntity)
                    {
                        await LocalizationService.AddNewLocalization(Localization);
                        if(CreationForClient)
                        {
                            /* po zapisaniu w localization jest przypisany id */
                            await CustomerService.AddNewCustomer(NewCustomer, Localization.Id, Credentials.ToCredentials());
                        }
                        else
                        {
                            await CustomerService.AddNewSeller(NewSeller, Localization.Id, Credentials.ToCredentials());
                        }
                    }
                    
                }
                catch
                {

                }
            }));

        #endregion
    }
}
