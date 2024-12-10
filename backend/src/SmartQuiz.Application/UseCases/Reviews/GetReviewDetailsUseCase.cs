

using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class GetReviewDetailsUseCase
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewDetailsUseCase(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ResultDto> Execute(Guid reviewId)
    {
        var review = await _reviewRepository.GetDetailsAsync(reviewId);
        if (review == null)
        {
            throw new NotFoundException("Avaliação não encontrada");
        }

        return new ResultDto(review);
    }
}
