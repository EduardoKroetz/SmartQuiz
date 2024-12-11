using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class GetUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserUseCase(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<ResultDto> Execute(Guid userId)
    {
        var user = await _userRepository.Query()
            .Include(x => x.Quizzes)
            .Include(x => x.Matches)
            .Select(x => new GetUserDto
            {
                Id = x.Id,
                Email = x.Email,
                Username = x.Username,
                MaxScore = x.Matches.Any() ? x.Matches.Max(m => m.Score) : 0,
                CreatedQuizzes = x.Quizzes.Count,
                MatchesPlayed = x.Matches.Count,
                TotalScore = x.Matches.Any() ? x.Matches.Sum(m => m.Score) : 0
            })
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null) 
            throw new NotFoundException("Usuário não encontrado");
        

        return new ResultDto(user);
    }
}