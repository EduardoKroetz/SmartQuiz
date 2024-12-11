using AutoMapper;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetMatchUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public GetMatchUseCase(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }
    
    public async Task<ResultDto> Execute(Guid matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null) 
            throw new ArgumentException("Não foi possível encontrar a partida");

        var matchDto = _mapper.Map<GetMatchDto>(match);
        
        return new ResultDto(matchDto);
    }
}