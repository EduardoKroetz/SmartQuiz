using SmartQuiz.Core.Entities.Base;

namespace SmartQuiz.Core.Repositories.Base;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    IQueryable<T> Query();
}