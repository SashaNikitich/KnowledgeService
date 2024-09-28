using KnowledgeService.Interfaces;
using KnowledgeService.Entities;
using KnowledgeService.Contexts;

public class UnitOfWork : IUnitOfWork
{
  private readonly ApplicationDbContext _dbContext;
  private bool disposed;

  private IRepository<Test> _testRepository;

  public IRepository<Test> Tests => _testRepository ??= new Repository<Test>(_dbContext);

  public UnitOfWork(ApplicationDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task SaveChangesAsync()
  {
    await _dbContext.SaveChangesAsync();
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  private void Dispose(bool disposing)
  {
    if (disposed)
    {
      return;
    }

    if (disposing)
    {
      _dbContext?.Dispose();
    }

    disposed = true;
  }

  ~UnitOfWork()
  {
    Dispose(false);
  }
}
