using AutoMapper;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetMatchUseCase
{
    private readonly IMapper _mapper;
    private readonly IMatchService _matchService;

    public GetMatchUseCase( IMapper mapper, IMatchService matchService)
    {
        _mapper = mapper;
        _matchService = matchService;
    }
    
    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchService.GetByIdAsync(matchId);
        if (match is null) 
            throw new ArgumentException("Não foi possível encontrar a partida");

        var matchDto = _mapper.Map<GetMatchDto>(match);
        
        return new ResultDto(matchDto);
    }
}