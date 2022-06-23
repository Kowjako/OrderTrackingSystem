using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.DTO
{
    public class ProcessDTO
    {
        public string Name { get; set; }
        public DateTime LastProcessDate { get; set; }
        public string Description { get; set; }

        public ProcessDTO(string procedure)
        {
            StoredProcedureFunction = procedure;
        }

        public readonly string StoredProcedureFunction;
    }
}
