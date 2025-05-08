namespace EXAM_SYSTEM_API.Domain.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(object id);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(List<TEntity> entity);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(List<TEntity> entity);
    Task DeleteAsync(object id);
}
