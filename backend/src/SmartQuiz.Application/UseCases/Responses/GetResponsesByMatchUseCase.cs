using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Responses;

public class GetResponsesByMatchUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IResponseService _responseService;


    public GetResponsesByMatchUseCase(IMatchRepository matchRepository, IResponseService responseService)
    {
        _matchRepository = matchRepository;
        _responseService = responseService;
    }

    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null) 
            throw new NotFoundException("Partida não encontrada");

        var responses = await _responseService.GetResponsesByMatch(match);

        return new ResultDto(responses);
    }
}