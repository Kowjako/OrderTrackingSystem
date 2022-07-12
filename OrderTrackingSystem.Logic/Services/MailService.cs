using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

/*
 * Używamy ForEachAsync bo użycie zwyklego ForEach z delegatem Action
 * powoduje wyjatki bo async ze zwracanym typem void nie oznacza ze to
 * sie wykona w przyszlosci
 */

namespace OrderTrackingSystem.Logic.Services
{
    public class MailService : CRUDManager, IMailService
    {
        private OrderService OrderService => new OrderService(new CustomerService(new ConfigurationService()));

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
                                SendDate = mail.Date,
                                Sender = customer.Name + " " + customer.Surname,
                                NadawcaMail = customer.Email,
                                SellerId = mail.SenderId,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value,
                            };
                var firstStageList = await query.AsNoTracking().ToListAsync();


                /* Podpinamy zamówienia */
                await firstStageList.ForEachAsync(async p =>
                {
                    var relationQuery = from relation in dbContext.MailOrderRelations
                                        join order in dbContext.Orders
                                        on relation.OrderId equals order.Id
                                        where relation.MailId == p.Id
                                        select order.Number;

                    p.RelatedOrders = await relationQuery.AsNoTracking().ToArrayAsync();
                });

                /* Skoro szukamy dla customera nie rozpatrywamy relacji SellerToCustomer */
                await firstStageList.ForEachAsync(async p =>
                {
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.ReceiverId);
                            p.Receiver = receiverCus.Name + " " + receiverCus.Surname;
                            p.OdbiorcaMail = receiverCus.Email;
                            break;
                        case (byte)MailDirectionType.CustomerToSeller:
                            var receiverSeller = await dbContext.Sellers.FindAsync(p.ReceiverId);
                            p.Receiver = receiverSeller.Name;
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
                                SendDate = mail.Date,
                                Receiver = customer.Name + " " + customer.Surname,
                                SellerId = mail.SenderId,
                                OdbiorcaMail = customer.Email,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value
                            };

                var firstStageList = await query.AsNoTracking().ToListAsync();

                /* Podpinamy zamówienia  */
                await firstStageList.ForEachAsync(async p =>
                {
                    var relationQuery = from relation in dbContext.MailOrderRelations
                                        join order in dbContext.Orders
                                        on relation.OrderId equals order.Id
                                        where relation.MailId == p.Id
                                        select order.Number;

                    p.RelatedOrders = await relationQuery.AsNoTracking().ToArrayAsync();
                });

                /* Nie rozpatrywamy relacji CustomerToSeller bo customer nie może być sellerem */
                await firstStageList.ForEachAsync(async p =>
                {
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.SellerId);
                            p.Sender = receiverCus.Name + " " + receiverCus.Surname;
                            p.NadawcaMail = receiverCus.Email;
                            break;
                        case (byte)MailDirectionType.SellerToCustomer:
                            var receiverSeller = await dbContext.Sellers.FindAsync(p.SellerId);
                            p.Sender = receiverSeller.Name;
                            p.NadawcaMail = receiverSeller.Email;
                            break;
                        default:
                            break;
                    }
                });

                return firstStageList;
            }
        }

        public async Task<List<MailDTO>> GetReceivedMailsForSeller(int sellerId)
        {
            /* Zakładamy że wyszukujemy dla sprzedawcy */
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Sellers.Where(c => c.Id == sellerId).FirstAsync();

                var query = from mail in dbContext.Mails
                            where mail.ReceiverId == sellerId && mail.MailRelation == 2
                            select new MailDTO
                            {
                                Id = mail.Id,
                                Caption = mail.Caption,
                                Content = mail.Content,
                                SendDate = mail.Date,
                                Receiver = customer.Name,
                                SellerId = mail.SenderId,
                                OdbiorcaMail = customer.Email,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value
                            };

                var firstStageList = await query.AsNoTracking().ToListAsync();

                /* Podpinamy zamówienia  */
                await firstStageList.ForEachAsync(async p =>
                {
                    var relationQuery = from relation in dbContext.MailOrderRelations
                                        join order in dbContext.Orders
                                        on relation.OrderId equals order.Id
                                        where relation.MailId == p.Id
                                        select order.Number;

                    p.RelatedOrders = await relationQuery.AsNoTracking().ToArrayAsync();
                });

                await firstStageList.ForEachAsync(async p =>
                {
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.CustomerToSeller:
                            var receiverCus = await dbContext.Customers.FindAsync(p.SellerId);
                            p.Sender = receiverCus.Name + " " + receiverCus.Surname;
                            p.NadawcaMail = receiverCus.Email;
                            break;
                        default:
                            break;
                    }
                });

                return firstStageList;
            }
        }

        public async Task<List<MailDTO>> GetSendMailsForSeller(int sellerId)
        {
            /* Zakładamy że wyszukujemy dla customer'a */
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Sellers.Where(c => c.Id == sellerId).FirstAsync();

                var query = from mail in dbContext.Mails
                            where mail.SenderId == sellerId && mail.MailRelation == 3
                            select new MailDTO
                            {
                                Id = mail.Id,
                                Caption = mail.Caption,
                                Content = mail.Content,
                                SendDate = mail.Date,
                                Sender = customer.Name,
                                NadawcaMail = customer.Email,
                                SellerId = mail.SenderId,
                                ReceiverId = mail.ReceiverId,
                                MailRelation = mail.MailRelation.Value,
                            };
                var firstStageList = await query.AsNoTracking().ToListAsync();

                /* Podpinamy zamówienia */
                await firstStageList.ForEachAsync(async p =>
                {
                    var relationQuery = from relation in dbContext.MailOrderRelations
                                        join order in dbContext.Orders
                                        on relation.OrderId equals order.Id
                                        where relation.MailId == p.Id
                                        select order.Number;

                    p.RelatedOrders = await relationQuery.AsNoTracking().ToArrayAsync();
                });

                await firstStageList.ForEachAsync(async p =>
                {
                    switch (p.MailRelation)
                    {
                        case (byte)MailDirectionType.SellerToCustomer:
                            var receiverCus = await dbContext.Customers.FindAsync(p.ReceiverId);
                            p.Receiver = receiverCus.Name + " " + receiverCus.Surname;
                            p.OdbiorcaMail = receiverCus.Email;
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
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
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

        public async Task GenerateAutomaticMessageAfterSend(int receiverId, int sellerId, string relatedSend = null)
        {
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    var customer = await dbContext.Customers.FirstAsync(p => p.Id == sellerId);

                    var mailDAL = new Mails
                    {
                        Caption = Properties.Resources.MailCaptionAfterSendAutomatic,
                        Content = string.Format(Properties.Resources.MailContentAfterSendAutomatic, relatedSend, customer.Name),
                        Date = DateTime.Now,
                        SenderId = sellerId,
                        ReceiverId = receiverId,
                        MailRelation = (int)MailDirectionType.CustomerToCustomer
                    };

                    dbContext.Mails.Add(mailDAL);
                    await dbContext.SaveChangesAsync();
                }
                transactionScope.Complete();
            }
        }

        public async Task SendComplaintMessage(int receiverId, int sellerId, int orderId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var customer = await dbContext.Customers.FirstAsync(p => p.Id == sellerId);
                var order = await dbContext.Orders.Include(p => p.ComplaintStates).FirstAsync(p => p.Id == orderId);

                var complaintState = order.ComplaintStates.FirstOrDefault();
                var definition = complaintState.ComplaintDefinitions; //EF6 lazy-loading

                var mailDAL = new Mails
                {
                    Caption = Properties.Resources.ComplaintSetHeader,
                    Content = string.Format(Properties.Resources.MailAutomaticSetComplaint, 
                                            order.Number, 
                                            definition?.ComplaintName ?? string.Empty, 
                                            customer.Name),
                    Date = DateTime.Now,
                    SenderId = sellerId,
                    ReceiverId = receiverId,
                    MailRelation = (int)MailDirectionType.CustomerToSeller
                };

                dbContext.Mails.Add(mailDAL);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddNewMail(Mails mail)
        {
           await base.AddEntity(mail);
        }
    }
}

