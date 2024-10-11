
using QuizDev.Application.DTOs.Matches;
using QuizDev.Application.DTOs.Responses;
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
        var match = await _matchRepository.GetAsync(matchId, false);
        if (match == null)
        {
            throw new ArgumentException("Não foi possível encontrar a partida");
        }

        if (match.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        var matchDto = new GetMatchDto(match.Id, match.Score, match.CreatedAt, match.Status, match.QuizId, match.Quiz, match.UserId, match.Reviewed, match.ReviewId);
        return new ResultDto(matchDto); 
    }
}
