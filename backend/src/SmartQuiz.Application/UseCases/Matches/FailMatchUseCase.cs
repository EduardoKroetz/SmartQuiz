using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class FailMatchUseCase
{
    private readonly IMatchRepository _matchRepository;

    public FailMatchUseCase(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid userId, Guid matchId)
    {
        var match = await _matchRepository.GetAsync(matchId);
        if (match is null)
        {
            throw new NotFoundException("Partida não encontrada");
        }

        if (match.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para finalizar essa partida");
        }

        match.Status = EMatchStatus.Failed;

        await _matchRepository.UpdateAsync(match);
        
        return new ResultDto(new {}); 
    }
}