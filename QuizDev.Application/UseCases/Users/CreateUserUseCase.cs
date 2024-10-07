using QuizDev.Application.Services;
using QuizDev.Core.DTOs.Users;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class CreateUserUseCase : IUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly AuthService _authService;

    public CreateUserUseCase(IUserRepository userRepository, AuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(CreateUserDto createUserDto)
    {
        var userExists = await _userRepository.GetByEmailAsync(createUserDto.Email);
        if (userExists != null)
        {
            throw new InvalidOperationException("Esse e-mail já está cadastrado");
        }

        var passwordHash = _authService.HashPassword(createUserDto.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = passwordHash,
        };

        await _userRepository.CreateAsync(user);

        var token = _authService.GenerateJwtToken(user);
        return new ResultDto(new { token, user.Id });
    }

}
