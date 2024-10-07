using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly QuizDevDbContext _dbContext;

    public QuizRepository(QuizDevDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Quiz quiz)
    {
        await _dbContext.Quizzes.AddAsync(quiz);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Quiz?> GetAsync(Guid id, bool includeQuestions = false)
    {
        var query = _dbContext.Quizzes.AsQueryable();

        if (includeQuestions)
        {
            query = query.Include(x => x.Questions).ThenInclude(x => x.Options);  
        }
        
        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

}
