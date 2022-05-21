using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class MailDTO
    {
        [Display(Name = "Caption", ResourceType = typeof(Properties.Resources))]
        public string Caption { get; set; }

        [Display(Name = "Content", ResourceType = typeof(Properties.Resources))]
        public string Content { get; set; }

        [Display(Name = "Sender", ResourceType = typeof(Properties.Resources))]
        public string Sender { get; set; }

        [LongDateField]
        [Display(Name = "SendDate", ResourceType = typeof(Properties.Resources))]
        public DateTime SendDate { get; set; }

        [Display(Name = "Receiver", ResourceType = typeof(Properties.Resources))]
        public string Receiver { get; set; }
    
        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public string NadawcaMail { get; set; }
        [Browsable(false)]
        public string OdbiorcaMail { get; set; }
        [Browsable(false)]
        public string NadawcaData => string.Format("{0} ({1})", Sender, NadawcaMail);
        [Browsable(false)]
        public string OdbiorcaData => string.Format("{0} ({1})", Receiver, OdbiorcaMail);

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

        #endregion

    }
}
