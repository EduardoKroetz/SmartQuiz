
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

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
