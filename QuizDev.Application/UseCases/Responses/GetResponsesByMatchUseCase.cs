
using QuizDev.Application.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Responses;

public class GetResponsesByMatchUseCase
{
    private readonly IResponseRepository _responseRepository;

    public GetResponsesByMatchUseCase(IResponseRepository responseRepository)
    {
        _responseRepository = responseRepository;
    }

    public async Task<ResultDto> Execute(Guid matchId)
    {
        var responses = await _responseRepository.GetResponsesByMatch(matchId);
        var dto = responses.Select(x => new GetResponseDto(x.Id, x.AnswerOptionId, x.AnswerOption, x.MatchId, x.IsCorrect));
        return new ResultDto(dto);
    }
}
