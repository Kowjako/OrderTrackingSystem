using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
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
                dbContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified; //use included attaching + mark dirty
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
        protected async virtual Task UpdateEntity<T>(T entity, params Expression<Func<T, object>> [] propertyToUpdate) where T : class
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Set<T>().Attach(entity);
                for (int i = 0; i < propertyToUpdate.Length; i++)
                {
                    dbContext.Entry(entity).Property(propertyToUpdate[i]).IsModified = true;
                }
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


        /// <summary>
        /// Zwraca wszystkie encje konkretnego typu
        /// </summary>
        /// <typeparam name="T">Typ encji</typeparam>
        /// <returns></returns>
        protected async virtual Task<List<T>> GetAllEntities<T>() where T : class
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                return await dbContext.Set<T>().ToListAsync();
            }
        }
    }
}
