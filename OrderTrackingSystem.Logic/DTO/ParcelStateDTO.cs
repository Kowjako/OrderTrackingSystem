using System;
using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class ParcelStateDTO
    {
        [Browsable(false)]
        public int StateId { get; set; }
        public string Name { get; set; }
        public DateTime Data { get; set; }
        public string Description { get; set; }
    }
}
