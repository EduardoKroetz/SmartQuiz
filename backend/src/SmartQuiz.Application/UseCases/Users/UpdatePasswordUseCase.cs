using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.DTOs.Users;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class UpdatePasswordUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UpdatePasswordUseCase(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid userId, UpdatePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        if (!_authService.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
        {
            throw new ArgumentException("Senha incorreta");
        }
        
        user.PasswordHash = _authService.HashPassword(dto.NewPassword);

        await _userRepository.UpdateAsync(user);

        return new ResultDto(new {});
    }
}