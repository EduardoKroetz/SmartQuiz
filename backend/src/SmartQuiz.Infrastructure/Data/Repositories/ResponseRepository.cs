using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;
using SmartQuiz.Infrastructure.Data.Repositories.Base;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class ResponseRepository : Repository<Response>, IResponseRepository
{
    public ResponseRepository(SmartQuizDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Response>> GetResponsesByMatch(Guid matchId)
    {
        return await context.Responses
            .Include(x => x.AnswerOption)
            .ThenInclude(x => x.Question)
            .ThenInclude(x => x.AnswerOptions)
            .Where(x => x.MatchId.Equals(matchId))
            .ToListAsync();
    }
}