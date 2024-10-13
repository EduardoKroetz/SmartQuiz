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
            query = query
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Select(x => new Quiz
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    UserId = x.UserId,
                    Expires = x.Expires,
                    ExpiresInSeconds = x.ExpiresInSeconds,
                    IsActive = x.IsActive,
                    Questions = x.Questions.OrderBy(q => q.Order).ToList()
                });  
        }
        
        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
    
    public async Task<List<Quiz>> SearchQuizByReviews(string[] keyWords, int skip, int take)
    {
        return await _dbContext.Reviews  //Filtra por Review
            .Include(x => x.Quiz)
            .Where(x =>  //Buscar os Quiz pelas palavras chaves
                keyWords.Any(k => x.Quiz.Title.ToLower().Contains(k.ToLower())) || //Buscar pelo título
                keyWords.Any(k => x.Quiz.Description.ToLower().Contains(k.ToLower())))    //Buscar pela descrição
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

    public async Task<List<Quiz>> SearchQuiz(string[] keyWords, int skip, int take)
    {
        return await _dbContext.Quizzes 
            .Where(x =>  //Buscar os Quiz pelas palavras chaves
                keyWords.Any(k => x.Title.ToLower().Contains(k.ToLower())) || //Buscar pelo título
                keyWords.Any(k => x.Description.ToLower().Contains(k.ToLower())))    //Buscar pela descrição
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
