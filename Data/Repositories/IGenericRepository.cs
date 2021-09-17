using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GestorTareas.Data.Repositories
{
    interface IGenericRepository<T> where T : class
    {
        public Task Add(T o);

        public Task AddRange(IEnumerable<T> o);

        public Task<T> Update(T o);

        public Task<bool> Remove(T o);

        public Task<T?> Get(Func<T, bool> predicate);

        public Task<List<T>> GetAll();

        public Task SaveChangesAsync();
    }
}
