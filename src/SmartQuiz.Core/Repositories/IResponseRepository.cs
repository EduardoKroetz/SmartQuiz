using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.Repositories;

public interface IResponseRepository
{
    Task CreateAsync(Response matchResponse);
    Task<List<Response>> GetResponsesByMatch(Guid matchId);
}
