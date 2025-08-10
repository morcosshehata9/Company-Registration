using CompanyRegistration.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyRegistration.Repository.GenericRepositories
{
    public class GenericRepository<T, Tkey> : IGenericRepository<T, Tkey> where T : class
    {
        private readonly AppDbContext Context; 
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            Context = appDbContext;
            _dbSet = Context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {

            return await _dbSet
                .Where(obj => EF.Property<bool>(obj, "IsDeleted") == false)
                .ToListAsync();

        }

        public async Task<T?> GetByIdAsync(Tkey id)
        {
            var obj = await _dbSet.FindAsync(id);

            if (obj != null)
            {
                var isDeletedProp = typeof(T).GetProperty("IsDeleted");
                if (isDeletedProp != null)
                {
                    var isDeleted = (bool)isDeletedProp.GetValue(obj)!;
                    if (isDeleted)
                        return null;
                }
                return obj;
            }

            return null;
        }
        public async Task AddAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }
        public void Update(T obj)
        {
            _dbSet.Update(obj);
        }

        public async Task<bool> DeleteAsync(Tkey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            typeof(T).GetProperty("IsDeleted").SetValue(entity, true);
            _dbSet.Update(entity);
            return true;


        }
        public IQueryable<T> GetQueryable() 
        {
            return _dbSet.AsQueryable();
        }
    }
}
