using AutoMapper;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Results;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class GetUserMatchesUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public GetUserMatchesUseCase(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResultDto> Execute(Guid userId, int pageSize, int pageNumber)
    {
        var skip = pageSize * (pageNumber - 1);
        var matches = await _matchRepository.GetUserMatchesAsync(userId, skip, pageSize);
        var matchesDto = _mapper.Map<IEnumerable<GetMatchDto>>(matches);
        return new PaginatedResultDto(pageSize, pageNumber, matches.Count, pageNumber + 1, matchesDto);
    }
}