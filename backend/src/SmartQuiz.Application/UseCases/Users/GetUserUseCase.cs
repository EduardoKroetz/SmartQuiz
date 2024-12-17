using AutoMapper;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Users;

public class GetUserUseCase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserUseCase(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid userId)
    {
        var user = await _userService.GetUserDetailsAsync(userId);

        if (user == null) 
            throw new NotFoundException("Usuário não encontrado");
        
        return new ResultDto(user);
    }
}