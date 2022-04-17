using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OrderTrackingSystem.Logic.DTO
{
    public class MailDTO
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Display(Name = "Caption", ResourceType = typeof(Properties.Resources))]
        public string Caption { get; set; }
        [Display(Name = "Content", ResourceType = typeof(Properties.Resources))]
        public string Content { get; set; }
        [Display(Name = "Sender", ResourceType = typeof(Properties.Resources))]
        public string Nadawca { get; set; }
        [Display(Name = "SendDate", ResourceType = typeof(Properties.Resources))]
        public string Date { get; set; }
        [Display(Name = "Receiver", ResourceType = typeof(Properties.Resources))]
        public string Odbiorca { get; set; }

        [Browsable(false)]
        public string NadawcaMail { get; set; }
        [Browsable(false)]
        public string OdbiorcaMail { get; set; }
        [Browsable(false)]
        public string NadawcaData => string.Format("{0} ({1})", Nadawca, NadawcaMail);
        [Browsable(false)]
        public string OdbiorcaData => string.Format("{0} ({1})", Odbiorca, OdbiorcaMail);

        [Browsable(false)]
        public string[] RelatedOrders { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int ReceiverId { get; set; }
        [Browsable(false)]
        public byte MailRelation { get; set; }

        [Browsable(false)]
        public bool HasRelatedOrders => RelatedOrders.Any();
    }
}
