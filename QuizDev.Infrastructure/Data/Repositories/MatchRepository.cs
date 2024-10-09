

using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly QuizDevDbContext _dbContext;
    private readonly IQuestionRepository _questionRepository;

    public MatchRepository(QuizDevDbContext dbContext, IQuestionRepository questionRepository)
    {
        _dbContext = dbContext;
        _questionRepository = questionRepository;
    }

    public async Task CreateAsync(Match match)
    {
        await _dbContext.Matches.AddAsync(match);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Question?> GetNextQuestion(Match match)
    {
        //Buscar a ultima questão respondida
        // Basicamente ele ordena de forma decrescente pela ordem
        // das questões no Quiz que está sendo jogado
        // e então pega o primeiro (em ordem decrescente)
        var lastResponse = await _dbContext
            .Responses
            .Where(x => x.MatchId.Equals(match.Id))
            .Include(x => x.QuestionOption)
                .ThenInclude(x => x.Question)
            .OrderByDescending(x => x.QuestionOption.Question.Order)
            .Select(x => new { x.QuestionOption.Question.Order })
            .FirstOrDefaultAsync();

        if (lastResponse == null)
            return null;

        //então pegar a próxima questão com base na ordem das questões
        return await _questionRepository.GetQuizQuestionByOrder(match.QuizId, lastResponse.Order + 1);
    }

    public async Task<Match?> GetAsync(Guid matchId, bool includeRelations)
    {
        var query = _dbContext.Matches.AsQueryable();

        if (includeRelations)
        {
            query = query
                .Include(x => x.Responses)
                .Include(x => x.Quiz)              
                .ThenInclude(x => x.Questions)
                .Select(x => new Match
                {
                    Id = x.Id,
                    QuizId = x.QuizId,
                    CreatedAt = x.CreatedAt,
                    Reviewed = x.Reviewed,
                    Score = x.Score,
                    UserId = x.UserId,
                    Status = x.Status,
                    ReviewId = x.ReviewId,
                    Responses = x.Responses,
                    Quiz = new Quiz
                    {
                        Id = x.Quiz.Id,
                        UserId = x.Quiz.UserId,
                        Description = x.Quiz.Description,
                        Expires = x.Quiz.Expires,
                        ExpiresInSeconds = x.Quiz.ExpiresInSeconds,
                        IsActive = x.Quiz.IsActive,
                        Title = x.Quiz.Title,
                        Questions = x.Quiz.Questions.Select(question => new Question
                        {
                            Id = question.Id,
                            QuizId = question.QuizId,
                            Text = question.Text,
                            Options = question.Options,
                            Order = question.Order,
                        }).ToList(),
                    }
                });
        }

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(matchId));
    }

    public async Task UpdateAsync(Match match)
    {
        _dbContext.Matches.Update(match);
        await _dbContext.SaveChangesAsync();
    }
}
