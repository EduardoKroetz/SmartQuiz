using QuizDev.Application.Services;
using QuizDev.Core.DTOs.Users;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class LoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly AuthService _authService;

    public LoginUserUseCase(IUserRepository userRepository, AuthService authService)
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
        return new ResultDto(new { token });
    }
}
