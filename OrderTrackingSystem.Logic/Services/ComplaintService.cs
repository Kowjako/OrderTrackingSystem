using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTrackingSystem.Logic.Services
{
    public class ComplaintService
    {
        public async Task<List<ComplaintFolderDTO>> GetComplaintFolders()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from folder in dbContext.ComplaintFolders
                            orderby folder.ParentComplaintFolderId
                            select folder;
                var folderList = await query.ToListAsync();

                var folderListDTO = folderList.Select(p =>
                new ComplaintFolderDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Children = new List<ComplaintFolderDTO>(),
                    ParentId = p.ParentComplaintFolderId
                }).ToList();

                /* Rekurencyjne wypelnianie drzewa */
                RecursiveTreeFiller<ComplaintFolderDTO>.FillTreeRecursive(folderListDTO);

                return folderListDTO.Where(p => p.ParentId == null).ToList();
            }
        }

        public async Task<List<ComplaintFolderDTO>> GetComplaintFoldersAll()
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

        public async Task<List<ComplaintsDTO>> GetComplaints()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from complaint in dbContext.ComplaintStates
                            join order in dbContext.Orders on
                            complaint.OrderId equals order.Id
                            select new ComplaintsDTO()
                            {
                                OrderNumber = order.Number,
                                State = complaint.State.ToString(),
                                Date = complaint.Date.Value.ToString(),
                                StateId = complaint.State,
                                SolutionDate = complaint.SolutionDate,
                                EndDate = complaint.EndDate
                            };
                var stagedList = await query.ToListAsync();
                stagedList.ForEach(p =>
                {
                    p.State = EnumConverter.GetNameById<ComplaintState>(int.Parse(p.State));
                    p.Date = DateTime.Parse(p.Date).ToShortDateString();
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

        public async Task SaveComplaintTemplate(ComplaintDefinitionDTO complaint, ComplaintFolderDTO folder)
        {
            var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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
                        ComplaintFolderId = folder.Id
                    };

                    dbContext.ComplaintRelations.Add(complaintFolderRelation);
                    await dbContext.SaveChangesAsync();
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
            var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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
            var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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

                    await dbContext.SaveChangesAsync();
                }
                transactionScope.Complete();
            }
        }

        public async Task RegisterNewComplaint(int complaintDefinitionId, int orderId)
        {

        }
    }
}

