using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;
using SmartQuiz.Infrastructure.Data.Repositories.Base;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    public QuestionRepository(SmartQuizDbContext dbContext) : base(dbContext)
    {
    }

    public new async Task<Question?> GetByIdAsync(Guid id)
    {
        return await context.Questions
            .Include(x => x.AnswerOptions)
            .Include(x => x.Quiz)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<Question?> GetQuizQuestionByOrder(Guid quizId, int order)
    {
        return await context
            .Questions
            .Include(x => x.AnswerOptions)
            .Where(x => x.QuizId.Equals(quizId))
            .Where(x => x.Order.Equals(order))
            .FirstOrDefaultAsync();
    }

    public async Task UpdateRangeAsync(List<Question> questions)
    {
        context.Questions.UpdateRange(questions);
        await context.SaveChangesAsync();
    }

    public async Task<List<Question>> GetQuestionsByQuizId(Guid quizId)
    {
        return await context.Questions
            .AsNoTracking()
            .Include(x => x.AnswerOptions)
            .Where(x => x.QuizId == quizId)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }
}