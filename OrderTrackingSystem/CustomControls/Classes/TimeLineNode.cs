using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.CustomControls.Classes
{
    internal enum TimeLineNodeState
    {
        Undefined = 0,
        Waiting = 1,
        Done = 2
    }

    internal sealed class TimeLineNode
    {
        public string Caption { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public TimeLineNode(string caption, string description)
        {
            Caption = caption;
            Description = description;
        }

        public TimeLineNode(string caption, DateTime date, string description) : this(caption, description)
        {
            Date = date;
        }

        public TimeLineNode(string caption, int year, 
                            int month, int day, int hour, 
                            int minute, int second, string description) : this(caption, description)
        {
            Date = new DateTime(year, month, day, hour, month, second);
        }
    }
}
