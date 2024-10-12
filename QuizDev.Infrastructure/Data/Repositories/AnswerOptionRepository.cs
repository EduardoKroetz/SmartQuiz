
using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class AnswerOptionRepository : IAnswerOptionRepository
{
    private readonly QuizDevDbContext _dbContext;

    public AnswerOptionRepository(QuizDevDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AnswerOption?> GetById(Guid id)
    {
        return await _dbContext.AnswerOptions.Include(x => x.Question).FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task CreateAsync(AnswerOption option)
    {
        await _dbContext.AnswerOptions.AddAsync(option);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(AnswerOption option)
    {
        _dbContext.AnswerOptions.Update(option);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(AnswerOption option)
    {
        _dbContext.AnswerOptions.Remove(option);
        await _dbContext.SaveChangesAsync();
    }
}
