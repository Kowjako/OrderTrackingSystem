using System;
using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class ParcelStateDTO
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int StateId { get; set; }

        #endregion
    }
}
