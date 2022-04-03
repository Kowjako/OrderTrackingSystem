using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class MailService : IService<MailService>
    {
        public async Task<List<MailDTO>> GetSendMailsForCustomer(int senderId)
        {
            /* Zakładamy że wyszukujemy dla customer'a */
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.Where(c => c.Id == senderId).FirstAsync();

                var query = from mail in dbContext.Mails
                            where mail.SenderId == senderId
                            select new MailDTO
                            {
                                Id = mail.Id,
                                Caption = mail.Caption,
                                Content = mail.Content,
                                Date = mail.Date.ToString(),
                                Nadawca = customer.Name + " " + customer.Surname,
                                SellerId = mail.SenderId,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value
                            };
                var firstStageList = await query.ToListAsync();

                /* Skoro szukamy dla customera nie rozpatrywamy relacji SellerToCustomer */
                firstStageList.ForEach(async p =>
                {
                    p.Date = DateTime.Parse(p.Date).ToShortDateString();
                    switch(p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.ReceiverId);
                            p.Odbiorca = receiverCus.Name + " " + receiverCus.Surname;
                            break;
                        case (byte)MailDirectionType.CustomerToSeller:
                            var receiverSeller = await dbContext.Sellers.FindAsync(p.ReceiverId);
                            p.Odbiorca = receiverSeller.Name;
                            break;
                        default:
                            break;
                    }
                });

                return firstStageList;
            }
        }

        public async Task<List<MailDTO>> GetReceivedMailsForCustomer(int receiverId)
        {
            /* Zakładamy że wyszukujemy dla customer'a */
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.Where(c => c.Id == receiverId).FirstAsync();

                var query = from mail in dbContext.Mails
                            where mail.ReceiverId == receiverId
                            select new MailDTO
                            {
                                Id = mail.Id,
                                Caption = mail.Caption,
                                Content = mail.Content,
                                Date = mail.Date.ToString(),
                                Odbiorca = customer.Name + " " + customer.Surname,
                                SellerId = mail.SenderId,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value
                            };
                var firstStageList = await query.ToListAsync();

                /* Nie rozpatrywamy relacji CustomerToSeller bo customer nie może być sellerem */
                firstStageList.ForEach(async p =>
                {
                    p.Date = DateTime.Parse(p.Date).ToShortDateString();
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.SellerId);
                            p.Nadawca = receiverCus.Name + " " + receiverCus.Surname;
                            break;
                        case (byte)MailDirectionType.SellerToCustomer:
                            var receiverSeller = await dbContext.Sellers.FindAsync(p.ReceiverId);
                            p.Nadawca = receiverSeller.Name;
                            break;
                        default:
                            break;
                    }
                });

                return firstStageList;
            }
        }
    }
}
