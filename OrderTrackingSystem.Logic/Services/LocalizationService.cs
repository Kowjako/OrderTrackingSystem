using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.Mappers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class LocalizationService : IService<Localizations>
    {
        public void Add(Localizations obj)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                context.Localizations.Add(obj);
                context.SaveChanges();
            }
        }

        public Localizations CreateNewInstance()
        {
            return new Localizations();
        }

        public bool Exists(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                return context.Localizations.Find(id) == null;
            }
        }

        public ICollection<Localizations> GetAll()
        {
            using(var context = new OrderTrackingSystemEntities())
            {
                return context.Localizations.ToList();
            }
        }

        public Localizations GetByPrimary(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                return context.Localizations.Find(id);
            }
        }

        public LocalizationRow GetRowByPrimary(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                return context.LocalizationRow.First(e => e.Id == id);
            }
        }

        public void Remove(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                context.Localizations.Remove(new Localizations { Id = id });
                context.SaveChanges();
            }
        }

        public void Update(Localizations obj)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Update(LocalizationRow obj)
        {
            var sourceLocalization = GlobalMapper.MapLocalizationRowToLocalization(obj);
            using (var context = new OrderTrackingSystemEntities())
            {
                context.Entry(sourceLocalization).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

    }
}
