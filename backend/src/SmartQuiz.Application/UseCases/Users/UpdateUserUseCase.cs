using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResultDto> Execute(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) 
            throw new NotFoundException("Usuário não encontrado");

        var userEmail = await _userRepository.GetByEmailAsync(updateUserDto.Email);
        if (userEmail != null && user.Email != userEmail.Email)
            throw new InvalidOperationException("Esse e-mail já está registrado");

        user.Email = updateUserDto.Email;
        user.Username = updateUserDto.Username;

        await _userRepository.UpdateAsync(user);

        return new ResultDto(new { });
    }
}