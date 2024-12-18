using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class LoginUserUseCase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public LoginUserUseCase(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<ResultDto> Execute(LoginUserDto loginUserDto)
    {
        var user = await _userService.GetByEmailAsync(loginUserDto.Email);
        if (user == null) 
            throw new ArgumentException("Email ou senha são inválidos");

        if (user.IsOAuthUser)
        {
            throw new InvalidOperationException("O e-mail está vinculado a uma conta Google. Faça login com o Google");
        }

        var isValid = _authService.VerifyPassword(loginUserDto.Password, user.PasswordHash);
        if (!isValid) 
            throw new ArgumentException("Email ou senha são inválidos");

        var token = _authService.GenerateJwtToken(user);
        return new ResultDto(new { Token = token });
    }
}