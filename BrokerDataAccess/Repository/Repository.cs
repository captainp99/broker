using System.Collections.Generic;

namespace BrokerDataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BrokerDBContext _context;
        
        public Repository(BrokerDBContext context)
        {
            _context = context;
        }
        public T Add(T obj)
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T Update(T t, params object[] key)
        {
            if (t == null)
            {
                return null;
            }

            T exist = _context.Set<T>().Find(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                _context.SaveChanges();
            }
            return exist;
        }

    }
}
