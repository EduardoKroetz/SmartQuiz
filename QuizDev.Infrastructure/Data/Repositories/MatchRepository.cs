

using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Enums;
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
            .Include(x => x.AnswerOption)
                .ThenInclude(x => x.Question)
            .OrderByDescending(x => x.AnswerOption.Question.Order)
            .Select(x => new { x.AnswerOption.Question.Order })
            .FirstOrDefaultAsync();

        if (lastResponse == null)
            return null;

        //então pegar a próxima questão com base na ordem das questões
        return await _questionRepository.GetQuizQuestionByOrder(match.QuizId, lastResponse.Order + 1);
    }

    public async Task<Match?> GetAsync(Guid matchId, bool includeRelations = false)
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
                        Title = x.Quiz.Title
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

    public async Task DeleteAsync(Match match)
    {
        _dbContext.Matches.Remove(match);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Match>> GetMatchesAsync(Guid userId, int skip, int take , string? reference = null, string? status = null, bool? reviewed = null, string? orderBy = null)
    {
        var query = _dbContext.Matches.AsQueryable();
        
        if (reference != null)
        {
            var keyWords = reference.ToLower().Split(" ");

            query = query.Where(x => keyWords.Any(k => 
                x.Quiz.Title.ToLower().Contains(k) ||
                x.Quiz.Description.ToLower().Contains(k)
            ));
        }

        if (status != null)
        {
            if (status == "created")
            {
                query = query.Where(x => x.Status.Equals(EMatchStatus.Created));
            }else if (status == "finished")
            {
                query = query.Where(x => x.Status.Equals(EMatchStatus.Finished));
            }
        }

        if (reviewed != null)
        {
            query = query.Where(x => x.Reviewed.Equals(reviewed));
        }
        
        if (orderBy != null)
        {
            if (orderBy == "score")
            {
                query = query.OrderBy(x => x.Score);
            }
            else if (orderBy == "created_at")
            {
                query = query.OrderBy(x => x.CreatedAt);
            }
        }else
        {
            query = query.OrderByDescending(x => x.Score);
        }

        return await query
            .Include(x => x.Quiz)
            .Where(x => x.UserId == userId)
            .Skip(skip)
            .Take(take)
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
                    Title = x.Quiz.Title  
                }
            })
            .ToListAsync();

    }
}
