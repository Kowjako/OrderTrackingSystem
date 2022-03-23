using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class TrackableItemDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Numer { get; set; }
        [DisplayName("Data utworzenia")]
        public string Data { get; set; }
        public string Nabywca { get; set; }
        public string Sprzedawca { get; set; }
        public string Kwota { get; set; }
    }
}
