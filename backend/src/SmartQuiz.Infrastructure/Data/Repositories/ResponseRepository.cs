using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class ResponseRepository : IResponseRepository
{
    private readonly SmartQuizDbContext _dbContext;

    public ResponseRepository(SmartQuizDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Response matchResponse)
    {
        await _dbContext.Responses.AddAsync(matchResponse);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<GetResponseDto>> GetResponsesByMatch(Guid matchId)
    {
        return await _dbContext.Responses
            .Include(x => x.AnswerOption)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.AnswerOptions)
            .Where(x => x.MatchId.Equals(matchId))
            .Select(x => new GetResponseDto(
                x.AnswerOption.QuestionId, 
                x.AnswerOptionId, 
                x.AnswerOption, 
                x.AnswerOption.Question.AnswerOptions.FirstOrDefault(x => x.IsCorrectOption), 
                x.IsCorrect))
            .ToListAsync();
    }
}
