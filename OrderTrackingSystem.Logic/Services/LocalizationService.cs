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
    public class LocalizationService : IBusinessService<Localizations>
    {
        public void Add(Localizations obj)
        {
            throw new NotImplementedException();
        }

        public Localizations CreateNewInstance()
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Localizations> GetAll()
        {
            throw new NotImplementedException();
        }

        public Localizations GetByPrimary(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                return context.Localizations.Find(id);
            }
        }

        public LocalizationRow GetLocalizationRowById(int id)
        {
            using(var context = new OrderTrackingSystemEntities())
            {
                return context.LocalizationRow.First(p => p.Id == id);
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
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
            using (var context = new OrderTrackingSystemEntities())
            {
                var originalLocalization = Mapper.ConvertToLocalizations(obj);
                context.Entry(originalLocalization).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
