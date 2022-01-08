using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OrderTrackingSystem.Logic.Services
{
    public class CustomerService : IBusinessService<Customers>
    {
        public void Add(Customers obj)
        {
            using(var context = new OrderTrackingSystemEntities())
            {
                context.Customers.Add(obj);
                context.Entry(obj).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public Customers CreateNewInstance()
        {
            return new Customers() { Number = Guid.NewGuid().ToString(), Balance = 500 };
        }

        public bool Exists(int id)
        {
            using(var context = new OrderTrackingSystemEntities())
            {
                return context.Customers.Find(id) != null;
            }
        }

        public ICollection<Customers> GetAll()
        {
            using(var context = new OrderTrackingSystemEntities())
            {
                return context.Customers.ToList();
            }
        }

        public Customers GetByPrimary(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                return context.Customers.Find(id);
            }
        }

        public void Remove(int id)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                context.Customers.Remove(context.Customers.Find(id));
                context.SaveChanges();
            }
        }

        public void Update(Customers obj)
        {
            using (var context = new OrderTrackingSystemEntities())
            {
                context.Entry(obj).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
