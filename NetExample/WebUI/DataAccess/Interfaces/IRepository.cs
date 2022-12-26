using System.Collections;

namespace DataAccess.Interfaces
{
    public interface IRepository<T>  where T :class,new()
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetAsync(int? id);

        public Task CreateAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);

        public Task SaveAsync();
    }
}
