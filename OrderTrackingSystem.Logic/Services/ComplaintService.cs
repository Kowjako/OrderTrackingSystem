﻿using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTrackingSystem.Logic.Services
{
    public class ComplaintService : CRUDManager, IComplaintService
    {
        private IMailService MailService => new MailService();
        private ITrackerService TrackerService => new TrackerService();

        public async Task<List<ComplaintFolderDTO>> GetComplaintFolders()
        {
            var folderListDTO = await GetComplaintFoldersWithoutComposing();

            /* Rekurencyjne wypelnianie drzewa */
            RecursiveTreeFiller<ComplaintFolderDTO>.FillTreeRecursive(folderListDTO);

            return folderListDTO.Where(p => p.ParentId == null).ToList();
        }

        public async Task<List<ComplaintFolderDTO>> GetComplaintFoldersWithoutComposing()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from folder in dbContext.ComplaintFolders
                            orderby folder.ParentComplaintFolderId
                            select folder;

                var folderList = await query.ToListAsync();
                var outputDTO = folderList.Select(p =>
                    new ComplaintFolderDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Children = new List<ComplaintFolderDTO>(),
                        ParentId = p.ParentComplaintFolderId
                    }
                );
                return outputDTO.ToList();
            }
        }

        public async Task<List<ComplaintsDTO>> GetComplaintsForCustomer(int customerId)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from complaint in dbContext.ComplaintStates
                            join order in dbContext.Orders on
                            complaint.OrderId equals order.Id
                            join complaintDefinition in dbContext.ComplaintDefinitions on
                            complaint.ComplaintDefinitionId equals complaintDefinition.Id
                            where order.CustomerId == customerId
                            select new ComplaintsDTO()
                            {
                                Id = complaint.Id,
                                OrderNumber = order.Number,
                                State = complaint.State.ToString(),
                                Date = complaint.Date.Value,
                                StateId = complaint.State,
                                SolutionDate = complaint.SolutionDate,
                                EndDate = complaint.EndDate,
                                OrderId = order.Id,
                                RemainDays = complaintDefinition.RemainDays
                            };
                var stagedList = await query.ToListAsync();
                stagedList.ForEach(p =>
                {
                    p.State = EnumConverter.GetNameById<ComplaintState>(int.Parse(p.State));
                });
                return stagedList;
            }
        }

        public async Task<List<ComplaintDefinitionDTO>> GetComplaintDefinitions()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from complaintRelation in dbContext.ComplaintRelations
                            join complaint in dbContext.ComplaintDefinitions
                            on complaintRelation.ComplaintId equals complaint.Id
                            join complaintFolder in dbContext.ComplaintFolders
                            on complaintRelation.ComplaintFolderId equals complaintFolder.Id
                            select new ComplaintDefinitionDTO()
                            {
                                Id = complaint.Id,
                                Name = complaint.ComplaintName,
                                RemainDays = complaint.RemainDays,
                                Definition = complaint.Definition,
                                ComplaintFolderId = complaintFolder.Id
                            };
                return await query.ToListAsync();
            }
        }

        public async Task SaveComplaintTemplate(ComplaintDefinitionDTO complaint, int folderId)
        {
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    var complaintDAL = new ComplaintDefinitions
                    {
                        ComplaintName = complaint.Name,
                        Definition = complaint.Definition,
                        RemainDays = complaint.RemainDays
                    };

                    dbContext.ComplaintDefinitions.Add(complaintDAL);

                    var complaintFolderRelation = new ComplaintRelations
                    {
                        ComplaintId = complaintDAL.Id,
                        ComplaintFolderId = folderId
                    };

                    dbContext.ComplaintRelations.Add(complaintFolderRelation);
                    await dbContext.SaveChangesAsync();

                    complaint.Id = complaintDAL.Id;
                }
                transactionScope.Complete();
            }
        }

        public async Task AddNewFolder(string name, ComplaintFolderDTO parentFolder)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                int? parentFolderId = parentFolder?.Id ?? null;

                var complaintDAL = new ComplaintFolders
                {
                    Name = name,
                    ParentComplaintFolderId = parentFolderId
                };

                dbContext.ComplaintFolders.Add(complaintDAL);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteWithAncestor(ComplaintFolderDTO complaintFolder)
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                /* Pobieramy id folderów głównego i podrzędnych */
                var allChildIds = RecursiveTreeFiller<ComplaintFolderDTO>.GetAllChild(complaintFolder).Select(p => p.Id).ToList();
                allChildIds.Add(complaintFolder.Id);

                /* Wybor relacji do usuniecia wskazujące na sub-foldery - robimy przed usunięciem folderów */
                var relationDeleteQuery = from rel in dbContext.ComplaintRelations
                                          where allChildIds.Contains(rel.ComplaintFolderId)
                                          select rel;

                var relationList = await relationDeleteQuery.ToListAsync();

                await DeleteChilds(complaintFolder, dbContext);

                /* Usuwamy relacje -> trigger usunie szablony */
                await relationList.ForEachAsync(async p =>
                {
                    var complaintRelation = new ComplaintRelations()
                    {
                        Id = p.Id
                    };
                    dbContext.ComplaintRelations.Attach(complaintRelation);
                    dbContext.Entry(complaintRelation).State = EntityState.Modified;
                    dbContext.ComplaintRelations.Remove(complaintRelation);
                    await dbContext.SaveChangesAsync();
                });
            }
        }

        private async Task DeleteChilds(ComplaintFolderDTO parent, OrderTrackingSystemEntities context)
        {
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
            {
                foreach (var child in parent.Children)
                {
                    if (!child.Children.Any())
                    {
                        var folder = new ComplaintFolders()
                        {
                            Id = child.Id
                        };
                        context.ComplaintFolders.Attach(folder);
                        context.Entry(folder).State = EntityState.Modified;
                        context.ComplaintFolders.Remove(folder);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        /* rekurencyjne usuwamy dzieci */
                        await DeleteChilds(child, context);
                    }
                }

                /* usuwamy rodzica bieżącego */
                var parentAbove = new ComplaintFolders()
                {
                    Id = parent.Id
                };
                context.ComplaintFolders.Attach(parentAbove);
                context.Entry(parentAbove).State = EntityState.Modified;
                context.ComplaintFolders.Remove(parentAbove);
                await context.SaveChangesAsync();

                transactionScope.Complete();
            }
        }

        public async Task DeleteAndMoveToAncestor(ComplaintFolderDTO complaintFolder)
        {
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
            {
                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    var childs = RecursiveTreeFiller<ComplaintFolderDTO>.GetAllChild(complaintFolder);
                    childs.ForEach(p =>
                    {
                        p.ParentId = complaintFolder.ParentId;
                        var folder = new ComplaintFolders()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            ParentComplaintFolderId = p.ParentId
                        };

                        dbContext.Entry(folder).State = EntityState.Modified;
                    });
                    await dbContext.SaveChangesAsync();

                    /* Wybieramy id szablonów znajdujace sie bezposrednio w tym folderze */
                    var templateIds = from rel in dbContext.ComplaintRelations
                                      join def in dbContext.ComplaintDefinitions
                                      on rel.ComplaintId equals def.Id
                                      where rel.ComplaintFolderId == complaintFolder.Id
                                      select rel;
                    var templatesInCurrent = await templateIds.ToListAsync();

                    templatesInCurrent.ForEach(p =>
                    {
                        p.ComplaintFolderId = complaintFolder.ParentId.Value;
                        dbContext.Entry(p).State = EntityState.Modified;
                    });

                    var complaintFolders = new ComplaintFolders { Id = complaintFolder.Id };
                    dbContext.Entry(complaintFolders).State = EntityState.Deleted; //robi automatycznie attach i oznacza jako usunieta

                    await dbContext.SaveChangesAsync();
                }
                transactionScope.Complete();
            }
        }

        public async Task RegisterNewComplaint(int complaintDefinitionId, int orderId)
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var complaintDAL = new ComplaintStates()
                {
                    OrderId = orderId,
                    State = 1,
                    Date = DateTime.Now,
                    ComplaintDefinitionId = complaintDefinitionId
                };

                dbContext.Entry(complaintDAL).State = EntityState.Added;
                dbContext.ComplaintStates.Add(complaintDAL);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ComplaintsDTO>> GetComplaintsForSeller(int sellerId)
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from complaint in dbContext.ComplaintStates
                            join order in dbContext.Orders on
                            complaint.OrderId equals order.Id
                            join complaintDefinition in dbContext.ComplaintDefinitions on
                            complaint.ComplaintDefinitionId equals complaintDefinition.Id
                            where order.SellerId == sellerId && !new[] { 0, 3 }.Contains(complaint.State)
                            select new ComplaintsDTO()
                            {
                                Id = complaint.Id,
                                OrderNumber = order.Number,
                                State = complaint.State.ToString(),
                                Date = complaint.Date.Value,
                                StateId = complaint.State,
                                EndDate = complaint.EndDate,
                                OrderId = order.Id,
                                RemainDays = complaintDefinition.RemainDays
                            };

                var preparedList = await query.AsNoTracking().ToListAsync();
                preparedList.ForEach(p =>
                    {
                        p.State = EnumConverter.GetNameById<ComplaintState>(int.Parse(p.State));
                    });
                return preparedList;
            }
        }

        public async Task UpdateComplaintState(ComplaintStates entity, int sellerId)
        {
            using(var scope = D3TransactionScope.GetTransactionScope())
            {
                await base.UpdateEntity(entity, entity => entity.SolutionDate, entity => entity.State); /* Zapisywanie zmodyfikowanej encji */

                var customerId = default(int);
                string sellerName = default(string);

                using (var dbContext = new OrderTrackingSystemEntities())
                {
                    customerId = dbContext.Customers.FirstOrDefault(p => p.Orders.Any(x => x.Id == entity.OrderId)).Id;
                    sellerName = dbContext.Sellers.FirstOrDefault(p => p.Id == sellerId).Name;
                    dbContext.ComplaintStates.Attach(entity);
                    await dbContext.Entry(entity).Reference(x => x.Orders).LoadAsync(); /* ładujemy zamówienie dla danej reklamacji */
                }

                var message = new Mails
                {
                    Caption = Properties.Resources.ComplaintWasResolved,
                    Content = string.Format(Properties.Resources.ComplaintResolvedTemplate, entity.Orders.Number, sellerName),
                    Date = DateTime.Now,
                    SenderId = sellerId,
                    ReceiverId = customerId,
                    MailRelation = (int)MailDirectionType.SellerToCustomer
                };

                await MailService.AddNewMail(message);
                await TrackerService.AddNewStateForOrder(entity.OrderId, OrderState.ComplaintResolved);

                scope.Complete();
            }
        }

        public async Task CloseComplaint(ComplaintStates entity)
        {
            entity.EndDate = DateTime.Now;
            entity.State = (int)ComplaintState.ComplaintSolved;
            await base.UpdateEntity(entity, entity => entity.State, entity => entity.EndDate);
        }
    }
}

