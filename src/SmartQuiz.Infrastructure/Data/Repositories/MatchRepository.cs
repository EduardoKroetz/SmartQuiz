using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.DTOs.Matches;
using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly SmartQuizDbContext _dbContext;
    private readonly IQuestionRepository _questionRepository;

    public MatchRepository(SmartQuizDbContext dbContext, IQuestionRepository questionRepository)
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

    public async Task<Match?> GetAsync(Guid matchId)
    {
        return await _dbContext.Matches
            .Include(x => x.Responses)
            .Include(x => x.Quiz)
            .ThenInclude(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == matchId);
    }

    public async Task<GetMatchDto?> GetDetailsAsync(Guid matchId)
    {
        return await _dbContext.Matches
            .Include(x => x.Responses)
            .Include(x => x.Quiz)
            .ThenInclude(x => x.Questions)
            .Select(x => new GetMatchDto
            {
                Id = x.Id,
                Score = x.Score,
                CreatedAt = x.CreatedAt,
                ExpiresIn = x.ExpiresIn,
                Status = x.Status.ToString(),
                QuizId = x.QuizId,
                UserId = x.UserId,
                Reviewed = x.Reviewed,
                ReviewId = x.ReviewId,
                Quiz = new GetQuizDto
                (
                    x.Quiz.Id,
                    x.Quiz.Title,
                    x.Quiz.Description,
                    x.Quiz.Expires,
                    x.Quiz.ExpiresInSeconds,
                    x.Quiz.IsActive,
                    x.Quiz.UserId
                )
            })
            .FirstOrDefaultAsync(x => x.Id == matchId);
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

    public async Task<List<GetMatchDto>> GetMatchesAsync(Guid userId, int skip, int take, string? reference = null, string? status = null, bool? reviewed = null, string? orderBy = null)
    {
        var query = _dbContext.Matches.AsQueryable();

        query = query.Include(x => x.Quiz);

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
            }
            else if (status == "finished")
            {
                query = query.Where(x => x.Status.Equals(EMatchStatus.Finished));
            }
            else if (status == "failed")
            {
                query = query.Where(x => x.Status.Equals(EMatchStatus.Failed));
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
        }
        else
        {
            query = query.OrderByDescending(x => x.Score);
        }

        return await query
            .Where(x => x.UserId == userId)
            .Skip(skip)
            .Take(take)
            .Select(x => new GetMatchDto
            {
                Id = x.Id,
                Score = x.Score,
                CreatedAt = x.CreatedAt,
                ExpiresIn = x.ExpiresIn,
                Status = x.Status.ToString(),
                QuizId = x.QuizId,
                UserId = x.UserId,
                Reviewed = x.Reviewed,
                ReviewId = x.ReviewId,
                Quiz = new GetQuizDto
                (
                    x.Quiz.Id,
                    x.Quiz.Title,
                    x.Quiz.Description,
                    x.Quiz.Expires,
                    x.Quiz.ExpiresInSeconds,
                    x.Quiz.IsActive,
                    x.Quiz.UserId
                )
            })
            .ToListAsync();

    }
}
