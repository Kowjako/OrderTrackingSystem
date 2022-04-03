using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public class MailDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Tytuł")]
        public string Caption { get; set; }
        [DisplayName("Treść")]
        public string Content { get; set; }
        public string Nadawca { get; set; }
        [DisplayName("Data wysyłki")]
        public string Date { get; set; }
        public string Odbiorca { get; set; }
        [Browsable(false)]
        public string[] RelatedOrders { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int ReceiverId { get; set; }
        [Browsable(false)]
        public byte MailRelation { get; set; }
    }
}
