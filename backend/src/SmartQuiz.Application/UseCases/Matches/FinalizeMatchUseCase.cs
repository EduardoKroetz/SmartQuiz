using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class FinalizeMatchUseCase
{
    private readonly IMatchRepository _matchRepository;

    public FinalizeMatchUseCase(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid matchId, Guid userId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null) throw new NotFoundException("Partida não encontrada");

        if (match.UserId != userId)
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");

        if (match.Status == EMatchStatus.Finished)
            throw new InvalidOperationException("Essa partida já foi finalizada");

        if (match.Status == EMatchStatus.Failed)
            throw new InvalidOperationException("Não é possível finalizar essa partida");

        match.Status = EMatchStatus.Finished;

        await _matchRepository.UpdateAsync(match);

        return new ResultDto(new { match.Id });
    }
}