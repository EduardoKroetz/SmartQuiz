
using QuizDev.Core.DTOs.Matches;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Matches;

public class GetMatchesUseCase
{
    private readonly IMatchRepository _matchRepository;

    public GetMatchesUseCase(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid userId, int pageSize, int pageNumber ,string? reference = null, string? status = null, bool? reviewed = null, string? orderBy = null)
    {
        var skip = pageSize * (pageNumber - 1);
        var matches = await _matchRepository.GetMatchesAsync(userId, skip, pageSize, reference, status, reviewed, orderBy);

        var dto = matches.Select(x => new GetMatchDto(x.Id, x.Score ,x.CreatedAt, x.ExpiresIn, x.Status, x.QuizId, x.Quiz , x.UserId, x.Reviewed, x.ReviewId));

        return new ResultDto(dto);
    }
}
