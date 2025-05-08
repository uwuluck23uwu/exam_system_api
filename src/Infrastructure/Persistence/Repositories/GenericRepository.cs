using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Domain.Interfaces;
using EXAM_SYSTEM_API.Infrastructure.Persistence;

namespace EXAM_SYSTEM_API.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly ExamSystemDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ExamSystemDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<List<TEntity>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public IQueryable<TEntity> GetAll()
    => _dbSet.AsQueryable();

    public async Task<TEntity?> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<TEntity> entity)
    {
        _dbSet.AddRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<TEntity> entity)
    {
        _dbSet.UpdateRange(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
