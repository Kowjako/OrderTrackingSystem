using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{ 
    public partial class TrackingViewModel
    {
        #region Bindable objects

        public Action ShowProgressBar;

        public List<TrackableItemDTO> Items { get; set; } = new List<TrackableItemDTO>();
        public ObservableCollection<ParcelStateDTO> ParcelStates { get; set; } = new ObservableCollection<ParcelStateDTO>();

        private TrackableItemDTO _selectedItem;

        public TrackableItemDTO SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnSelectedItemChanged();
            }
        }

        public CustomerDTO Customer { get; set; }
        public CustomerDTO Seller { get; set; }

        /* Filtering bindings */
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int ItemsSelection { get; set; }

        public List<ComplaintDefinitionDTO> ComplaintDefinitionsList { get; set; }
        public ComplaintDefinitionDTO SelectedComplaint { get; set; }

        public bool SentMessageWithComplaint { get; set; } = false;

        #endregion
    }
}
