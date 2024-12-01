
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class DeleteReviewUseCase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMatchRepository _matchRepository;

    public DeleteReviewUseCase(IReviewRepository reviewRepository, IMatchRepository matchRepository)
    {
        _reviewRepository = reviewRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid reviewId, Guid userId)
    {
        var review = await _reviewRepository.GetById(reviewId);
        if (review == null)
        {
            throw new NotFoundException("Avaliação não encontrada");
        }

        if (review.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        await _reviewRepository.DeleteAsync(review);

        review.Match.Reviewed = false;

        await _matchRepository.UpdateAsync(review.Match);

        return new ResultDto(new { review.Id });
    }
}
