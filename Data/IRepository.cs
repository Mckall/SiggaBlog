namespace SiggaBlog.Data;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T item);
    Task AddAllAsync(IEnumerable<T> item);
    Task UpdateAsync(T item);
    Task DeleteAsync(int id);
    Task AddOrUpdateListAsync(IEnumerable<T> posts);
}