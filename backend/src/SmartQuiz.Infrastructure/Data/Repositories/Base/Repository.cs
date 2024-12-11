using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities.Base;
using SmartQuiz.Core.Repositories.Base;

namespace SmartQuiz.Infrastructure.Data.Repositories.Base;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected SmartQuizDbContext context;

    public Repository(SmartQuizDbContext context)
    {
        this.context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public IQueryable<T> Query()
    {
        return context.Set<T>().AsQueryable();
    }
}