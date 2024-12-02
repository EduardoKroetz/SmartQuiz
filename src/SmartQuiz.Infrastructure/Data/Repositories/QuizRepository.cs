using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly SmartQuizDbContext _dbContext;

    public QuizRepository(SmartQuizDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Quiz quiz)
    {
        await _dbContext.Quizzes.AddAsync(quiz);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(quiz).State = EntityState.Detached;
    }

    public async Task<Quiz?> GetAsync(Guid id, bool includeQuestions = false)
    {
        var query = _dbContext.Quizzes.AsQueryable();

        if (includeQuestions)
        {
            query = query
                .Include(x => x.Questions)
                .ThenInclude(x => x.AnswerOptions)
                .Select(x => new Quiz
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    UserId = x.UserId,
                    Expires = x.Expires,
                    ExpiresInSeconds = x.ExpiresInSeconds,
                    IsActive = x.IsActive,
                    Difficulty = x.Difficulty,
                    Theme = x.Theme,
                    Questions = x.Questions.OrderBy(q => q.Order).ToList()
                });
        }

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<List<Quiz>> SearchQuizByReviews(string[]? keyWords, int skip, int take)
    {
        var query = _dbContext.Reviews.AsQueryable();

        query = query.Include(x => x.Quiz);

        if (keyWords != null)
        {
            query = query.Where(x => keyWords.Any(k =>
                x.Quiz.Title.ToLower().Contains(k) ||
                x.Quiz.Description.ToLower().Contains(k)
            ));
        }

        return await query
            .Where(x => x.Quiz.IsActive == true)
            .OrderBy(x => x.Rating) //Ordernar as Reviews pela maior avaliação
            .Skip(skip)
            .Take(take)
            .Select(x => new Quiz //Selecionar o Quiz dessas avaliações
            {
                Id = x.Quiz.Id,
                Title = x.Quiz.Title,
                Description = x.Quiz.Description,
                UserId = x.Quiz.UserId,
                Expires = x.Quiz.Expires,
                ExpiresInSeconds = x.Quiz.ExpiresInSeconds,
                IsActive = x.Quiz.IsActive,
            })
            .ToListAsync();
    }

    public async Task<List<Quiz>> SearchQuiz(string[]? keyWords, int skip, int take)
    {
        var query = _dbContext.Quizzes.AsQueryable();

        if (keyWords != null)
        {
            query = query.Where(x => keyWords.Any(k =>
                x.Title.ToLower().Contains(k) ||
                x.Description.ToLower().Contains(k)
            ));
        }

        return await query
            .Where(x => x.IsActive == true)
            .Skip(skip)
            .Take(take)
            .Select(x => new Quiz //Selecionar o Quiz dessas avaliações
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                UserId = x.UserId,
                Expires = x.Expires,
                ExpiresInSeconds = x.ExpiresInSeconds,
                IsActive = x.IsActive,
            })
            .ToListAsync();
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        _dbContext.Quizzes.Update(quiz);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Quiz quiz)
    {
        _dbContext.Quizzes.Remove(quiz);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> HasMatchesRelated(Guid quizId)
    {
        return await _dbContext.Matches.AnyAsync(x => x.QuizId == quizId);
    }
}
