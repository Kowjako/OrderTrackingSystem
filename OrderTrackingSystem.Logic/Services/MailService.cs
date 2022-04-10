using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTrackingSystem.Logic.Services
{
    public class MailService : IService<MailService>
    {
        private OrderService OrderService => new OrderService();

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
                                NadawcaMail = customer.Email,
                                SellerId = mail.SenderId,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value,
                            };
                var firstStageList = await query.AsNoTracking().ToListAsync();

                /* Podpinamy zamówienia */
                firstStageList.ForEach(async p =>
                {
                    var relationQuery = from relation in dbContext.MailOrderRelations
                                        join order in dbContext.Orders
                                        on relation.OrderId equals order.Id
                                        where relation.MailId == p.Id
                                        select order.Number;

                    p.RelatedOrders = await relationQuery.AsNoTracking().ToArrayAsync();
                });

                /* Skoro szukamy dla customera nie rozpatrywamy relacji SellerToCustomer */
                firstStageList.ForEach(async p =>
                {
                    p.Date = DateTime.Parse(p.Date).ToShortDateString();
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.ReceiverId);
                            p.Odbiorca = receiverCus.Name + " " + receiverCus.Surname;
                            p.OdbiorcaMail = receiverCus.Email;
                            break;
                        case (byte)MailDirectionType.CustomerToSeller:
                            var receiverSeller = await dbContext.Sellers.FindAsync(p.ReceiverId);
                            p.Odbiorca = receiverSeller.Name;
                            p.OdbiorcaMail = receiverSeller.Email;
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
                                OdbiorcaMail = customer.Email,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value
                            };

                var firstStageList = await query.AsNoTracking().ToListAsync();

                /* Podpinamy zamówienia */
                firstStageList.ForEach(async p =>
                {
                    var relationQuery = from relation in dbContext.MailOrderRelations
                                        join order in dbContext.Orders
                                        on relation.OrderId equals order.Id
                                        where relation.MailId == p.Id
                                        select order.Number;

                    p.RelatedOrders = await relationQuery.AsNoTracking().ToArrayAsync();
                });

                /* Nie rozpatrywamy relacji CustomerToSeller bo customer nie może być sellerem */
                firstStageList.ForEach(async p =>
                {
                    p.Date = DateTime.Parse(p.Date).ToShortDateString();
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.SellerId);
                            p.Nadawca = receiverCus.Name + " " + receiverCus.Surname;
                            p.NadawcaMail = receiverCus.Email;
                            break;
                        case (byte)MailDirectionType.SellerToCustomer:
                            var receiverSeller = await dbContext.Sellers.FindAsync(p.ReceiverId);
                            p.Nadawca = receiverSeller.Name;
                            p.NadawcaMail = receiverSeller.Email;
                            break;
                        default:
                            break;
                    }
                });

                return firstStageList;
            }
        }

        public async Task SendMail(MailDTO mail, string[] relatedOrders = null)
        {
            var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    var mailDAL = new Mails
                    {
                        Caption = mail.Caption,
                        Content = mail.Content,
                        Date = DateTime.Now,
                        SenderId = mail.SellerId,
                        ReceiverId = mail.ReceiverId,
                        MailRelation = mail.MailRelation
                    };

                    dbContext.Mails.Add(mailDAL);
                    await dbContext.SaveChangesAsync();

                    /* Pobieramy zamówienia po numerach */
                    var ordersToLink = await OrderService.GetOrdersListByCodes(relatedOrders);

                    /* Tworzymy relacje i dodajemy do kontekstu */
                    ordersToLink.ForEach(p =>
                    {
                        dbContext.MailOrderRelations.Add(new MailOrderRelations()
                        {
                            MailId = mailDAL.Id,
                            OrderId = p.Id
                        });
                    });

                    await dbContext.SaveChangesAsync();
                };
                transactionScope.Complete();
            }
        }
    }
}
