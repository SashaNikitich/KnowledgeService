using KnowledgeService.Interfaces;
using KnowledgeService.Entities;

public interface IUnitOfWork : IDisposable
{
  IRepository<Test> Tests { get; }
  Task SaveChangesAsync();
}
