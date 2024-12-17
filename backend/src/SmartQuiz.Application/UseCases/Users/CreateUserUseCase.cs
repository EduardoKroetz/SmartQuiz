using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class CreateUserUseCase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public CreateUserUseCase(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<ResultDto> Execute(CreateUserDto createUserDto)
    {
        var userExists = await _userService.GetByEmailAsync(createUserDto.Email);
        if (userExists != null) 
            throw new InvalidOperationException("Esse e-mail já está cadastrado");
        
        var user = _userService.CreateUser(createUserDto.Username, createUserDto.Email, createUserDto.Password);
        await _userService.AddAsync(user);

        var token = _authService.GenerateJwtToken(user);
        return new ResultDto(new { Token = token, user.Id });
    }
}