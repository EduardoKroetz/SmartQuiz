using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Matches;

public class DeleteMatchUseCase
{
    private readonly IMatchService _matchService;
    private readonly IAuthService _authService;

    public DeleteMatchUseCase(IMatchService matchService, IAuthService authService)
    {
        _matchService = matchService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid matchId, Guid userId)
    {
        var match = await _matchService.GetByIdAsync(matchId);
        if (match == null) 
            throw new NotFoundException("Partida não encontrada");

        _authService.ValidateSameUser(match.Quiz.UserId, userId);

        await _matchService.DeleteAsync(match);

        return new ResultDto(new { match.Id });
    }
}