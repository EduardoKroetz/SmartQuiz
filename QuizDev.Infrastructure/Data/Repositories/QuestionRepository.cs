using Microsoft.EntityFrameworkCore;
using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly QuizDevDbContext _dbContext;

    public QuestionRepository(QuizDevDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Question question)
    {
        await _dbContext.Questions.AddAsync(question);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Question?> GetAsync(Guid id, bool includeRelations = false)
    {
        var query = _dbContext.Questions.AsQueryable();

        if (includeRelations)
        {
            query = query.Include(x => x.Options).Include(x => x.Quiz);
        }

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<Question?> GetQuizQuestionByOrder(Guid quizId, int order)
    {
        return await _dbContext
            .Questions
                .Include(x => x.Options)
            .Where(x => x.QuizId.Equals(quizId))
            .Where(x => x.Order.Equals(order))
            .Select(x => new Question { Id = x.Id, Text = x.Text, Order = x.Order , Options = x.Options, QuizId = x.QuizId })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateRangeAsync(List<Question> questions)
    {
        _dbContext.Questions.UpdateRange(questions);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<GetQuestionDto>> GetQuestionByQuizId(Guid quizId)
    {
        return await _dbContext.Questions
            .Include(x => x.Options)
            .Where(x => x.QuizId == quizId)
            .Select(x => new GetQuestionDto(x.Id, x.Text, x.QuizId, x.Order, x.Options))
            .OrderBy(x => x.Order)
            .ToListAsync();
    }
}
