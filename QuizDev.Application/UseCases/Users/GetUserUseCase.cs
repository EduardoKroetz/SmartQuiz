
using QuizDev.Application.Exceptions;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class GetUserUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResultDto> Execute(Guid userId)
    {
        var user = await _userRepository.GetDetailsAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        return new ResultDto(user);
    }
}
