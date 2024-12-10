using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.DTOs.Users;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class LoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public LoginUserUseCase(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(LoginUserDto loginUserDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);
        if (user == null)
        {
            throw new ArgumentException("Email ou senha são inválidos");
        }

        var isValid = _authService.VerifyPassword(loginUserDto.Password, user.PasswordHash);
        if (!isValid)
        {
            throw new ArgumentException("Email ou senha são inválidos");
        }

        var token = _authService.GenerateJwtToken(user);
        return new ResultDto(new { Token = token });
    }
}
