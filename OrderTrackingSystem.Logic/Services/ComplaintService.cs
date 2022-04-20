using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class ComplaintService
    {
        public async Task<List<ComplaintFolderDTO>> GetComplaintFolders()
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from folder in dbContext.ComplaintFolders
                            orderby folder.ParentComplaintFolderId
                            select folder;
                var folderList = await query.ToListAsync();

                List<ComplaintFolderDTO> foldersOutput = new List<ComplaintFolderDTO>();

                /* Realizacja wzorca Composite */
                foreach (var folder in folderList)
                {
                    if (!folder.ParentComplaintFolderId.HasValue)
                    {
                        foldersOutput.Add(new ComplaintFolderDTO()
                        {
                            Id = folder.Id,
                            Name = folder.Name,
                            Children = new List<ComplaintFolderDTO>()
                        });
                    }
                    else
                    {
                        foldersOutput.First(p => p.Id == folder.ParentComplaintFolderId).Children.Add(new ComplaintFolderDTO()
                        {
                            Id = folder.Id,
                            Name = folder.Name,
                            Children = new List<ComplaintFolderDTO>()
                        });
                    }
                }
                return foldersOutput;
            }
        }

        public async Task<List<ComplaintsDTO>> GetComplaints()
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from complaint in dbContext.ComplaintStates
                            join order in dbContext.Orders on
                            complaint.OrderId equals order.Id
                            select new ComplaintsDTO()
                            {
                                OrderNumber = order.Number,
                                State = complaint.State.ToString(),
                                Date = complaint.Date.Value.ToString()
                            };
                var stagedList =  await query.ToListAsync();
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
    }
}
