using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    /// <summary>
    /// Klasa abstrakcyjna wykorzystywana do ułatwienia operacji standardowych
    /// na bazie dotyczących CRUD
    /// </summary>
    public abstract class CRUDManager
    {
        /// <summary>
        /// Modyfikuje rekord na bazie
        /// </summary>
        /// <typeparam name="T">Typ encji</typeparam>
        /// <param name="entity">Rekord</param>
        protected async virtual Task UpdateEntity<T>(T entity) where T : class
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Usuwa rekord na bazie
        /// </summary>
        /// <typeparam name="T">Typ encji</typeparam>
        /// <param name="entity">Rekord</param>
        protected async virtual Task DeleteEntity<T>(T entity) where T : class
        { 
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
                await dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Dodaje nowy rekord do bazy
        /// </summary>
        /// <typeparam name="T">Typ encji</typeparam>
        /// <param name="entity">Rekord</param>
        /// <returns></returns>
        protected async virtual Task AddEntity<T>(T entity) where T : class
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
