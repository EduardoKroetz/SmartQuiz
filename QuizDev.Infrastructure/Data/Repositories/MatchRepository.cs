

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
        var lastResponse = await _dbContext
            .MatchResponses
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

}
