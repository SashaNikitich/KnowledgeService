using System.Linq.Expressions;
using KnowledgeService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeService.Repositories;

public sealed class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _entity;

    public Repository(DbContext context)
    {
        _context = context;
        _entity = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity?>> GetAllAsync()
    {
        return await _entity.AsNoTracking().ToListAsync();
    }

    public IQueryable<TEntity?> GetAllAsync(Expression<Func<TEntity?, bool>> predicate)
    {
        return _entity.Where(predicate);
    }

    public async Task<TEntity?> GetAsync(int id)
    {
        return await _entity.FindAsync(id);
    }

    public async Task<TEntity?> CreateAsync(TEntity? item)
    {
        await _entity.AddAsync(item);
        return item;
    }

    public async Task<int> CountAsync()
    {
        return await _entity.CountAsync();
    }

    public void Delete(TEntity? entity)
    {
        _entity.Remove(entity);
    }

    public void Update(TEntity item)
    {
        _context.Update(item);
    }
}
