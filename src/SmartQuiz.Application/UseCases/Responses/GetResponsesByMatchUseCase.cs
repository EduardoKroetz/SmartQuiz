
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Responses;

public class GetResponsesByMatchUseCase
{
    private readonly IResponseRepository _responseRepository;
    private readonly IMatchRepository _matchRepository;

    public GetResponsesByMatchUseCase(IResponseRepository responseRepository, IMatchRepository matchRepository)
    {
        _responseRepository = responseRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchRepository.GetAsync(matchId);
        if (match == null)
        {
            throw new NotFoundException("Partida não encontrada");
        }

        var responses = await _responseRepository.GetResponsesByMatch(matchId);
        var dto = responses.Select(x => new GetResponseDto(x.Id, x.AnswerOptionId, x.AnswerOption, x.MatchId, x.IsCorrect));
        return new ResultDto(dto);
    }
}
