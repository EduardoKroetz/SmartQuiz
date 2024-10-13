
using Microsoft.EntityFrameworkCore;
using QuizDev.Core.DTOs.AnswerOptions;
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

    public async Task<List<GetAnswerOptionDto>> GetByQuestionId(Guid questionId)
    {
        return await _dbContext.AnswerOptions
            .AsNoTracking()
            .Where(x => x.QuestionId == questionId)
            .Select(x => new GetAnswerOptionDto(x.Id, x.Response, x.QuestionId))
            .ToListAsync();
    }
}
