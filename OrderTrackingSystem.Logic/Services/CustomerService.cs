using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class CustomerService : IBusinessService<Customers>
    {
        public void Add(Customers obj)
        {
            using(var context = new OrderTrackingSystemEntities())
            {
                context.Customers.Add(obj);
            }
        }

        public Customers CreateNewInstance()
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Customers> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Customers> GetByPrimary(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Customers obj)
        {
            throw new NotImplementedException();
        }
    }
}
