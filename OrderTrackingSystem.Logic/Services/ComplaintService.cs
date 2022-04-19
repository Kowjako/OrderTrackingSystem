﻿using OrderTrackingSystem.Logic.DataAccessLayer;
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
                                State = EnumConverter.GetNameById<ComplaintState>(complaint.State),
                                Date = complaint.Date
                            };
            }
        }
    }
}
