using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;

namespace EXAM_SYSTEM_API.Application.Interfaces;

public interface IGenericCrudService<TEntity> where TEntity : class
{
    Task<PaginationResponse<TEntity>> GetAllPageAsync(int pageSize, int currentPage);
    Task<List<TEntity>> GetAllAsync();
    IQueryable<TEntity> GetAll();
    Task<ResponseMessage> AddAsync(TEntity entity);
    Task<ResponseMessage> AddRangeAsync(List<TEntity> entity);
    Task<ResponseMessage> UpdateAsync(TEntity entity);
    Task<ResponseMessage> UpdateRangeAsync(List<TEntity> entity);
    Task<ResponseMessage> DeleteAsync(object id);
    Task<TEntity?> GetByIdAsync(object id);
}
