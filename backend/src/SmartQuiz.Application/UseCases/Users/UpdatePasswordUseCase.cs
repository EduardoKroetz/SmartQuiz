using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class UpdatePasswordUseCase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    
    public UpdatePasswordUseCase(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<ResultDto> Execute(Guid userId, UpdatePasswordDto dto)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user == null) 
            throw new NotFoundException("Usuário não encontrado");

        if (!_authService.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
            throw new ArgumentException("Senha incorreta");

        user.PasswordHash = _authService.HashPassword(dto.NewPassword);

        await _userService.UpdateAsync(user);

        return new ResultDto(new { });
    }
}