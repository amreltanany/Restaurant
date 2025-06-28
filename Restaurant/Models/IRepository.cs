namespace Restaurant.Models
{
    public interface IRepository<T> where T : class
    {
        Task <IEnumerable<T>> GetAll();
        Task<T> GetById(int id, QueryOptions<T> options);
        Task Add(T entity );
        Task Update(T entity);
        Task DeleteAsync(int id);
    }
}
