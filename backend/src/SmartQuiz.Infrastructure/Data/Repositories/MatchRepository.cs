using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;
using SmartQuiz.Infrastructure.Data.Repositories.Base;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class MatchRepository : Repository<Match>, IMatchRepository
{
    private readonly IQuestionRepository _questionRepository;

    public MatchRepository(SmartQuizDbContext dbContext, IQuestionRepository questionRepository) : base(dbContext)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Question?> GetNextQuestion(Match match)
    {
        //Buscar a ultima questão respondida
        // Basicamente ele ordena de forma decrescente pela ordem
        // das questões no Quiz que está sendo jogado
        // e então pega o primeiro (em ordem decrescente)
        var lastResponse = await context
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

    public new async Task<Match?> GetByIdAsync(Guid matchId)
    {
        return await context.Matches
            .Include(x => x.Responses)
            .Include(x => x.Quiz)
            .ThenInclude(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == matchId);
    }

    public async Task<List<Match>> GetMatchesAsync(Guid userId, int skip, int take, string? reference = null,
        string? status = null, bool? reviewed = null, string? orderBy = null)
    {
        var query = context.Matches.AsQueryable();

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
                query = query.Where(x => x.Status.Equals(EMatchStatus.Created));
            else if (status == "finished")
                query = query.Where(x => x.Status.Equals(EMatchStatus.Finished));
            else if (status == "failed") query = query.Where(x => x.Status.Equals(EMatchStatus.Failed));
        }

        if (reviewed != null) query = query.Where(x => x.Reviewed.Equals(reviewed));

        if (orderBy != null)
        {
            if (orderBy == "score")
                query = query.OrderBy(x => x.Score);
            else if (orderBy == "created_at") query = query.OrderBy(x => x.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(x => x.Score);
        }

        return await query
            .Where(x => x.UserId == userId)
            .Skip(skip)
            .Take(take)
            .OrderByDescending(x => x.CreatedAt)
            .Include(x => x.Quiz)
            .ThenInclude(x => x.Questions)
            .ToListAsync();
    }

    public async Task<List<Match>> GetUserMatchesAsync(Guid userId, int skip, int take)
    {
        return await context.Matches
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Skip(skip)
            .Take(take)
            .Include(x => x.Quiz)
            .ThenInclude(x => x.Questions)
            .ToListAsync();
    }
}