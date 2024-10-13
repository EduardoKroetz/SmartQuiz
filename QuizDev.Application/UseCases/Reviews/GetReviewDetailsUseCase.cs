

using QuizDev.Application.Exceptions;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.DTOs.Reviews;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Reviews;

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
