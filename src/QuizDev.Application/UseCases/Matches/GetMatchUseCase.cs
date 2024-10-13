
using QuizDev.Core.DTOs.Matches;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Matches;

public class GetMatchUseCase
{
    private readonly IMatchRepository _matchRepository;

    public GetMatchUseCase(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid matchId, Guid userId)
    {
        var match = await _matchRepository.GetDetailsAsync(matchId);
        if (match == null)
        {
            throw new ArgumentException("Não foi possível encontrar a partida");
        }

        if (match.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        return new ResultDto(match); 
    }
}
