using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using EnumConverter = OrderTrackingSystem.Logic.EnumMappers.EnumConverter;

namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class DesktopViewModel : BaseViewModel, IDesktopViewModel
    {
        #region Services

        private readonly IMailService MailService;
        private readonly IComplaintService ComplaintService;
        private readonly IOrderService OrderService;
        private readonly ICustomerService CustomerService;
        private readonly IProductService ProductService;
        private readonly ITrackerService TrackerService;

        #endregion

        #region Ctor

        public DesktopViewModel()
        {
            MailService = new MailService();
            ComplaintService = new ComplaintService();
            OrderService = new OrderService();
            CustomerService = new CustomerService();
            ProductService = new ProductService();
            TrackerService = new TrackerService();
        }

        #endregion

        #region Private members

        private Sellers CurrentSeller { get; set; }

        private void SetAvailableStates()
        {
            //TODO : stan zrobic sortowany po dacie
            var context = new FSMContext((OrderState)SelectedOrder.CurrentOrderState); /* ustawiamy obecny stan automatu */
            ParcelAvailableStates.Clear();
            foreach (var state in context.State.GetNextStates())
            {
                ParcelAvailableStates.Add(new Tuple<string, OrderState, int>
                                         (EnumConverter.GetNameById<OrderState>((int)state.Item1), state.Item1, state.Item2));
                ParcelAvailableStates = new List<Tuple<string, OrderState, int>>(ParcelAvailableStates);
            }
            OnPropertyChanged(nameof(ParcelAvailableStates));
        }

        private byte[] _imageData;

        #endregion

        #region Bindable properties

        public List<MailDTO> SentMessages { get; set; }
        public List<MailDTO> ReceivedMessages { get; set; }
        public MailDTO CurrentMail { get; set; } = new MailDTO();
        public List<OrderDTO> CustomersOrder { get; set; }
        public List<ComplaintsDTO> CustomersComplaint { get; set; }
        public List<Tuple<string, OrderState, int>> ParcelAvailableStates { get; set; } = new List<Tuple<string, OrderState, int>>();
        public Tuple<string, OrderState, int> SelectedState { get; set; }

        public Products CurrentProduct { get; set; } = new Products();
        public List<CategoryDTO> ProductCategories { get; set; }
        public CategoryDTO SelectedCategory { get; set; }

        public Visibility SplitterVisibility { get; set; } = Visibility.Hidden;
        public double ActualMailHeight { get; set; } = 0;

        private MailDTO _selectedMail;
        public MailDTO SelectedMail
        {
            get => _selectedMail;
            set
            {
                _selectedMail = value;
                OnPropertyChanged(nameof(SelectedMail));
                ActualMailHeight = double.NaN; /* equals to Height = Auto */
                SplitterVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(ActualMailHeight));
                OnPropertyChanged(nameof(SplitterVisibility));
            }
        }

        private OrderDTO _selectedOrder;
        public OrderDTO SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                SetAvailableStates();
            }
        }

        #endregion

        #region Public methods

        public override async Task SetInitializeProperties()
        {
            CurrentSeller = await CustomerService.GetCurrentSeller();
            CustomersOrder = await OrderService.GetOrdersFromCompany(CurrentSeller.Id);
            CustomersComplaint = await ComplaintService.GetComplaintsForSeller(CurrentSeller.Id);
            ReceivedMessages = await MailService.GetReceivedMailsForSeller(CurrentSeller.Id);
            SentMessages = await MailService.GetSendMailsForSeller(CurrentSeller.Id);
            ProductCategories = await ProductService.GetProductSubCategories();
            OnManyPropertyChanged(new[] {nameof(ParcelAvailableStates), nameof(CustomersOrder), nameof(CustomersComplaint),
                                         nameof(ReceivedMessages), nameof(SentMessages), nameof(ProductCategories)});
        }

        #endregion

        #region Commands

        private RelayCommand _addProduct;

        public RelayCommand AddProduct =>
            _addProduct ??= new RelayCommand(async obj =>
            {
                try
                {
                    CurrentProduct.Category = SelectedCategory?.Id ?? -1;
                    CurrentProduct.SellerId = CurrentSeller.Id;
                    if(ValidatorWrapper.ValidateWithResult(new ProductValidator(), CurrentProduct))
                    {
                        await ProductService.SaveNewProduct(CurrentProduct, _imageData);
                        ShowSuccess("Produkt pomyślnie dodany");
                    }
                    else
                    {
                        ShowWarning(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    ShowError("Błąd podczas dodawania produktu");
                }
            });

        private RelayCommand _approveComplaint;
        public RelayCommand ApproveComplaint =>
            _approveComplaint ??= new RelayCommand(async obj =>
            {
                if(obj is null)
                {
                    ShowWarning("Należy zaznaczyć reklamację");
                    return;
                }

                var complaintDTO = obj as ComplaintsDTO;
                var complaint = new ComplaintStates
                {
                    Id = complaintDTO.Id,
                    SolutionDate = DateTime.Now,
                    State = (byte)complaintDTO.StateId,
                    OrderId = complaintDTO.OrderId
                };

                await ComplaintService.UpdateComplaintState(complaint, CurrentSeller.Id);
                ShowSuccess("Reklamacja została zatwierdzona");
            });

        private RelayCommand _sendMessage;
        public RelayCommand SendMessage =>
            _sendMessage ??= new RelayCommand(async obj =>
            {
                if (ValidatorWrapper.ValidateWithResult(new MailValidator(), CurrentMail))
                {
                    var customer = await CustomerService.GetCustomerByMail(CurrentMail.OdbiorcaMail);
                    var mail = new Mails()
                    {
                        Caption = CurrentMail.Caption,
                        Content = CurrentMail.Content,
                        Date = DateTime.Now,
                        MailRelation = (int)MailDirectionType.SellerToCustomer,
                        SenderId = CurrentSeller.Id,
                        ReceiverId = customer.Id
                    };

                    await MailService.AddNewMail(mail);
                    ShowSuccess("Wiadomość została wysłana");
                }
                else
                {
                    ShowWarning(ValidatorWrapper.ErrorMessage);
                }
            });

        private RelayCommand _changeParcelState;
        public RelayCommand ChangeParcelState =>
            _changeParcelState ??= new RelayCommand(async obj =>
            {
                if(SelectedOrder != null)
                {
                    await TrackerService.AddNewStateForOrder(SelectedOrder.Id, SelectedState.Item2);
                    ShowSuccess("Status przesyłki został zmieniony");
                }
                else
                {
                    ShowWarning("Należy wybrać zamówienie");
                }
            });

        private RelayCommand _addPicture;
        public RelayCommand AddPicture =>
            _addPicture ??= new RelayCommand(obj =>
            {
                _imageData = ImageDataHelper.LoadImage();
            });

        #endregion

    }
}
