using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels.Common
{
    public abstract class BaseViewModel : INotifyableViewModel, INotifyPropertyChanged
    {
        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        public abstract Task SetInitializeProperties();

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
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

        public virtual void ShowSuccess(string msg)
        {
            OnSuccess?.Invoke(msg);
        }

        public virtual void ShowWarning(string msg)
        {
            OnWarning?.Invoke(msg);
        }

        public virtual void ShowError(string msg)
        {
            OnFailure?.Invoke(msg);
        }
    }
}
