using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Domain.Interfaces;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Services;

public class GenericCrudService<TEntity> : IGenericCrudService<TEntity> where TEntity : class
{
    private readonly IGenericRepository<TEntity> repo;

    public GenericCrudService(IGenericRepository<TEntity> _repo)
    {
        repo = _repo;
    }

    public Task<TEntity?> GetByIdAsync(object id) => repo.GetByIdAsync(id);

    public async Task<PaginationResponse<TEntity>> GetAllPageAsync(int pageSize, int currentPage)
    {
        var all = await repo.GetAllAsync();
        var totalCount = all.Count;
        var paged = all
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginationResponse<TEntity>(paged, totalCount, currentPage, pageSize);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public IQueryable<TEntity> GetAll()
    {
        return repo.GetAll();
    }

    public async Task<ResponseMessage> AddAsync(TEntity entity)
    {
        await repo.AddAsync(entity);
        return new ResponseMessage(200, true, "Created successfully");
    }

    public async Task<ResponseMessage> AddRangeAsync(List<TEntity> entity)
    {
        await repo.AddRangeAsync(entity);
        return new ResponseMessage(200, true, "Created successfully");
    }

    public async Task<ResponseMessage> UpdateAsync(TEntity entity)
    {
        await repo.UpdateAsync(entity);
        return new ResponseMessage(200, true, "Updated successfully");
    }

    public async Task<ResponseMessage> UpdateRangeAsync(List<TEntity> entity)
    {
        await repo.UpdateRangeAsync(entity);
        return new ResponseMessage(200, true, "Updated successfully");
    }

    public async Task<ResponseMessage> DeleteAsync(object id)
    {
        await repo.DeleteAsync(id);
        return new ResponseMessage(200, true, "Deleted successfully");
    }

}