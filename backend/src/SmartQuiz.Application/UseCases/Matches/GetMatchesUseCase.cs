using AutoMapper;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Results;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetMatchesUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;
    
    public GetMatchesUseCase(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResultDto> Execute(Guid userId, int pageSize, int pageNumber, string? reference = null,
        string? status = null, bool? reviewed = null, string? orderBy = null)
    {
        var skip = pageSize * (pageNumber - 1);
        var matches = await _matchRepository.GetMatchesAsync(userId, skip, pageSize, reference, status, reviewed, orderBy);
        
        var matchesDto = _mapper.Map<IEnumerable<GetMatchDto>>(matches);

        return new PaginatedResultDto(pageSize, pageNumber, matches.Count, pageNumber + 1, matchesDto);
    }
}