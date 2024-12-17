using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Matches;

public class FinalizeMatchUseCase
{
    private readonly IMatchService _matchService;
    private readonly IAuthService _authService;

    public FinalizeMatchUseCase(IMatchService matchService, IAuthService authService)
    {
        _matchService = matchService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid matchId, Guid userId)
    {
        var match = await _matchService.GetByIdAsync(matchId);
        if (match is null) 
            throw new NotFoundException("Partida não encontrada");
        
        _authService.ValidateSameUser(match.UserId, userId);
        
        _matchService.FinalizeMatch(match);
        
        await _matchService.UpdateAsync(match);

        return new ResultDto(new { match.Id });
    }
}