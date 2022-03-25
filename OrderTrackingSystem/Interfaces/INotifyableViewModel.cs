using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface INotifyableViewModel
    {
        event Action<string> OnSuccess;
        event Action<string> OnFailure;
        event Action<string> OnWarning;
    }
}
