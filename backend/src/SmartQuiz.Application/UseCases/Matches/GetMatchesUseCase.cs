using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.DTOs.Results;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Matches;

public class GetMatchesUseCase
{
    private readonly IMatchService _matchService;

    public GetMatchesUseCase(IMatchService matchService)
    {
        _matchService = matchService;
    }

    public async Task<PaginatedResultDto> Execute(Guid userId, GetMatchesDto dto)
    {
        var matches = await _matchService.GetMatchesAsync(dto, userId);
        return new PaginatedResultDto(dto.PageSize, dto.PageNumber, matches.Count(), dto.PageNumber + 1, matches);
    }
}