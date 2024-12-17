using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserService _userService;

    public UpdateUserUseCase(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ResultDto> Execute(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user == null) 
            throw new NotFoundException("Usuário não encontrado");

        var userEmail = await _userService.GetByEmailAsync(updateUserDto.Email);
        if (userEmail != null && user.Email != userEmail.Email)
            throw new InvalidOperationException("Esse e-mail já está registrado");

        _userService.UpdateUser(user, updateUserDto);

        await _userService.UpdateAsync(user);

        return new ResultDto(new { });
    }
}