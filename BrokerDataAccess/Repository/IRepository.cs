using System.Collections.Generic;

namespace BrokerDataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        T Add(T obj);
        IEnumerable<T> GetAll();
        T Get(int id);
        T Update(T t, params object[] key);
    }
}
