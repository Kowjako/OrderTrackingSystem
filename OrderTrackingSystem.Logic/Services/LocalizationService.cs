using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class LocalizationService : IBusinessService<LocalizationRow>
    {
        public void Add(LocalizationRow obj)
        {
            throw new NotImplementedException();
        }

        public LocalizationRow CreateNewInstance()
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<LocalizationRow> GetAll()
        {
            throw new NotImplementedException();
        }

        public LocalizationRow GetByPrimary(int id)
        {
            throw new NotImplementedException();
        }

        public LocalizationRow GetLocalizationById(int id)
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

        public void Update(LocalizationRow obj)
        {
            throw new NotImplementedException();
        }
    }
}
