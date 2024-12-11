using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class CreateUserUseCase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(CreateUserDto createUserDto)
    {
        var userExists = await _userRepository.GetByEmailAsync(createUserDto.Email);
        if (userExists != null) throw new InvalidOperationException("Esse e-mail já está cadastrado");

        var passwordHash = _authService.HashPassword(createUserDto.Password);
        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.AddAsync(user);

        var token = _authService.GenerateJwtToken(user);
        return new ResultDto(new { Token = token, user.Id });
    }
}