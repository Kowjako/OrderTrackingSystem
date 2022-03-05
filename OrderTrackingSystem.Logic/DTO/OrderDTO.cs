using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class OrderDTO
    {
        public string Numer { get; set; }
        public string Oplata { get; set; }
        public string Dostawa { get; set; }
        public string Rezygnacja { get; set; }
        public string Kwota { get; set; }
        public string Sklep { get; set; }
    }
}
