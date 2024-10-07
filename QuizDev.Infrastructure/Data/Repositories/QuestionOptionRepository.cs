
using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class QuestionOptionRepository : IQuestionOptionRepository
{
    private readonly QuizDevDbContext _dbContext;

    public QuestionOptionRepository(QuizDevDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<QuestionOption?> GetById(Guid id)
    {
        return await _dbContext.QuestionOptions.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task CreateAsync(QuestionOption option)
    {
        await _dbContext.QuestionOptions.AddAsync(option);
        await _dbContext.SaveChangesAsync();
    }
}
