using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public interface IBusinessService<T>
    {
        T GetByPrimary(int id);
        ICollection<T> GetAll();

        void Add(T obj);
        void Remove(int id);
        void Update(T obj);

        bool Exists(int id);

        T CreateNewInstance();
    }
}
