
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Responses;

public class GetResponsesByMatchUseCase
{
    public GetResponsesByMatchUseCase(IResponseRepository responseRepository, IMatchRepository matchRepository)
    {
        _responseRepository = responseRepository;
        _matchRepository = matchRepository;
    }

    private readonly IResponseRepository _responseRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IAnswerOptionRepository _answerOptionRepository;
    
    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchRepository.GetAsync(matchId);
        if (match == null)
        {
            throw new NotFoundException("Partida não encontrada");
        }

        var responses = await _responseRepository.GetResponsesByMatch(matchId);
        
        return new ResultDto(responses);
    }
}
