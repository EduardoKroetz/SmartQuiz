
using QuizDev.Core.DTOs.Results;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class GetUserQuizzesUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserQuizzesUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResultDto> Execute(Guid userId, int pageSize, int pageNumber)
    {
        var skip = pageSize * ( pageNumber - 1 );
        var quizzes = await _userRepository.GetUserQuizzesAsync(userId, skip, pageSize);
        return new PaginatedResultDto(pageSize, pageNumber, quizzes.Count, pageNumber + 1, quizzes);
    }

}
