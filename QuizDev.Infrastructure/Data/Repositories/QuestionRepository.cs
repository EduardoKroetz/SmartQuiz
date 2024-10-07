using Microsoft.EntityFrameworkCore;
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
}
