using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
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
        /// Modyfikuje wybraną właściowość encji
        /// </summary>
        /// <typeparam name="T">Typ encji</typeparam>
        /// <typeparam name="V">Typ modyfikowanej właściwości</typeparam>
        /// <param name="entity">Encja</param>
        /// <param name="propertyToUpdate">Lambda która wybiera zmodyfikowany parametr</param>
        protected async virtual Task UpdateEntity<T, V>(T entity, Expression<Func<T, V>> propertyToUpdate) where T : class
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Entry(entity).Property(propertyToUpdate).IsModified = true;
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

        /// <summary>
        /// Zwraca encję wykorzystując selektor, przykładowo może to być Id encji
        /// </summary>
        /// <typeparam name="T">Typ encji</typeparam>
        /// <param name="selector">Selektor do wyboru encji</param>
        protected async virtual Task<T> GetEntity<T>(Expression<Func<T, bool>> selector) where T : class
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                return await dbContext.Set<T>().FirstOrDefaultAsync(selector);
            }
        }
    }
}
