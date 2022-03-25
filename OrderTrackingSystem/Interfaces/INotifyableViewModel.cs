using System;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface INotifyableViewModel
    {
        event Action<string> OnSuccess;
        event Action<string> OnFailure;
        event Action<string> OnWarning;
    }
}
