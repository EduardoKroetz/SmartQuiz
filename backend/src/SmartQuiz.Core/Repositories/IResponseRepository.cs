using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.Repositories;

public interface IResponseRepository
{
    Task CreateAsync(Response matchResponse);
    Task<List<GetResponseDto>> GetResponsesByMatch(Guid matchId);
}
