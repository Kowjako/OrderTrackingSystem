/* zeby nie bylo konfliktow z DAL OrderState */
using OS = OrderTrackingSystem.Logic.Services.OrderState;

namespace OrderTrackingSystem.Logic.HelperClasses
{
    public class ParcelStateFSM
    {
        private readonly OS _currentState;

        public ParcelStateFSM(int orderState)
        {
            _currentState = (OS)orderState;
        }

    }
}
