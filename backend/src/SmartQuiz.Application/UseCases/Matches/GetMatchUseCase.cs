
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetMatchUseCase
{
    private readonly IMatchRepository _matchRepository;

    public GetMatchUseCase(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchRepository.GetDetailsAsync(matchId);
        if (match == null)
        {
            throw new ArgumentException("Não foi possível encontrar a partida");
        }

        return new ResultDto(match);
    }
}
