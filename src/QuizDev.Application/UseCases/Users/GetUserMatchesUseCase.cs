
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.DTOs.Results;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class GetUserMatchesUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserMatchesUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResultDto> Execute(Guid userId, int pageSize, int pageNumber)
    {
        var skip = pageSize * ( pageNumber - 1 );
        var matches = await _userRepository.GetUserMatchesAsync(userId, skip, pageSize);
        return new PaginatedResultDto(pageSize, pageNumber, matches.Count, pageNumber + 1 ,matches);
    }
}
