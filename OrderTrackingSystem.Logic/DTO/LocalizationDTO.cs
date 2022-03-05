using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class LocalizationDTO
    {
        public string Kraj { get; set; }
        public string Miasto { get; set; }
        public string Ulica { get; set; }
        public int Mieszkanie { get; set; }
        public int Budynek { get; set; }
        public string Kod { get; set; }
    }
}
