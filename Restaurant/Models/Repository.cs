using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
namespace Restaurant.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // ApplicationDbContext is the context class that represents a session with the database.
        protected ApplicationDbContext _context { get; set; }

        // DbSet<T> represents a collection of entities of type T in the context.
        private DbSet<T> _dbSet { get; set; }

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
         
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            if(options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            if(options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }
            foreach(string include in options.GetIncludes())
            {
                query = query.Include(include);
            }
            var key= _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string PrimaryKeyName = key?.Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, PrimaryKeyName) == id);

        }

        public async Task Update(T entity)
        {
           _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllByIdAsync<TKey>(TKey id, string propertyName, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;

            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }


            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }

            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }
            // Filter by the specified property name and id
            query = query.Where(e => EF.Property<TKey>(e, propertyName).Equals(id));

            return await query.ToListAsync();
        }
}
}
