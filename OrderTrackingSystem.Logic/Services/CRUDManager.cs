using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public abstract class CRUDManager
    {
        public async virtual Task UpdateEntity<T>(T entity) where T : class
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        public async virtual Task DeleteEntity<T>(T entity) where T : class
        { 
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
        }


        public async virtual Task AddEntity<T>(T entity) where T : class
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
