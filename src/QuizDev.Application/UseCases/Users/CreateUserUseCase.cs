using QuizDev.Core.DTOs.Users;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;
using QuizDev.Application.Services.Interfaces;

namespace QuizDev.Application.UseCases.Users;

public class CreateUserUseCase : IUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public CreateUserUseCase(IUserRepository userRepository, IAuthService authService)
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
        return new ResultDto(new { Token = token, user.Id });
    }

}
