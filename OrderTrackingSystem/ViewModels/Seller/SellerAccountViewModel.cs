using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class SellerAccountViewModel : ISellerAccountViewModel, INotifyableViewModel
    {
        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;
    }
}
