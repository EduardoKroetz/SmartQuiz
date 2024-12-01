using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.DTOs.Questions;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly SmartQuizDbContext _dbContext;

    public QuestionRepository(SmartQuizDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Question question)
    {
        await _dbContext.Questions.AddAsync(question);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Question?> GetAsync(Guid id, bool includeRelations = true)
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
            .Select(x => new Question { Id = x.Id, Text = x.Text, Order = x.Order, Options = x.Options, QuizId = x.QuizId })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateRangeAsync(List<Question> questions)
    {
        _dbContext.Questions.UpdateRange(questions);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<GetQuestionDto>> GetQuestionsByQuizId(Guid quizId)
    {
        return await _dbContext.Questions
            .AsNoTracking()
            .Where(x => x.QuizId == quizId)
            .Select(x => new GetQuestionDto
            {
                Id = x.Id,
                Text = x.Text,
                QuizId = x.QuizId,
                Order = x.Order,
                Options = x.Options.Select(o => new GetAnswerOptionDto(o.Id, o.Response, o.QuestionId)).ToList()
            })
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<GetQuestionDto?> GetQuestionDetails(Guid questionId)
    {
        return await _dbContext.Questions
            .AsNoTracking()
            .Select(x => new GetQuestionDto
            {
                Id = x.Id,
                Text = x.Text,
                QuizId = x.QuizId,
                Order = x.Order,
                Options = x.Options.Select(o => new GetAnswerOptionDto(o.Id, o.Response, o.QuestionId)).ToList()
            })
            .FirstOrDefaultAsync(x => x.Id == questionId);
    }

    public async Task UpdateAsync(Question question)
    {
        _dbContext.Questions.Update(question);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Question question)
    {
        _dbContext.Questions.Remove(question);
        await _dbContext.SaveChangesAsync();
    }
}
