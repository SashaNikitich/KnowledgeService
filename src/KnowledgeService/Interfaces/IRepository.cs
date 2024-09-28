using System.Linq.Expressions;

namespace KnowledgeService.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
  Task<TEntity?> GetAsync(int id);

  Task<IEnumerable<TEntity?>> GetAllAsync();

  IQueryable<TEntity?> GetAllAsync(Expression<Func<TEntity?, bool>> predicate);

  Task<TEntity?> CreateAsync(TEntity? item);

  Task<int> CountAsync();

  void Update(TEntity item);

  void Delete(TEntity entity);
}
