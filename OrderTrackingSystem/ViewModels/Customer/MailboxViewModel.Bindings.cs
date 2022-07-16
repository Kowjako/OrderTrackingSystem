using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class MailboxViewModel
    {
        #region Bindable objects

        public MailDTO OriginalMail { get; set; } = new MailDTO();
        public CustomerDTO CurrentSender { get; set; }
        public List<MailDTO> ReceivedMessages { get; set; }
        public List<MailDTO> SentMessages { get; set; }
        public CustomerDTO MailReceiver { get; set; }
        public ObservableCollection<string> RelatedToCurrentMailOrders { get; set; } = new ObservableCollection<string>();
        public List<OrderDTO> CustomerOrders { get; set; }
        public OrderDTO SelectedOrder { get; set; }

        public DateTime DateFrom { get; set; } = DateTime.Now;
        public DateTime DateTo { get; set; } = DateTime.Now;
        public int SelectedFilterMsgType = -1;
        public DesignerSerializationVisibility SplitterVisibility { get; set; } = DesignerSerializationVisibility.Hidden;

        public double ActualMailHeight { get; set; } = 0;

        private MailDTO _selectedMail;
        public MailDTO SelectedMail
        {
            get => _selectedMail;
            set
            {
                _selectedMail = value;
                OnPropertyChanged(nameof(SelectedMail));
                ActualMailHeight = Double.NaN; /* equals to Height = Auto */
                SplitterVisibility = DesignerSerializationVisibility.Visible;
                OnPropertyChanged(nameof(ActualMailHeight));
                OnPropertyChanged(nameof(SplitterVisibility));
            }
        }

        #endregion
    }
}
