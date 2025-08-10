
namespace CompanyRegistration.Repository.GenericRepositories
{
    public interface IGenericRepository<T, Tkey>  where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Tkey id);
        Task AddAsync(T obj);
        void Update(T obj);
        Task<bool> DeleteAsync(Tkey id);
        IQueryable<T> GetQueryable();
    }
}
