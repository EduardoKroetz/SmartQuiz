using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;
using SmartQuiz.Infrastructure.Data.Repositories.Base;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class AnswerOptionRepository : Repository<AnswerOption>, IAnswerOptionRepository
{
    public AnswerOptionRepository(SmartQuizDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<AnswerOption>> GetByQuestionId(Guid questionId)
    {
        return await context.AnswerOptions
            .AsNoTracking()
            .Where(x => x.QuestionId == questionId)
            .ToListAsync();
    }
}