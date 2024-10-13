
using QuizDev.Application.Exceptions;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.DTOs.Reviews;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Reviews;

public class UpdateReviewUseCase
{
    private readonly IReviewRepository _reviewRepository;

    public UpdateReviewUseCase(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ResultDto> Execute(UpdateReviewDto dto, Guid reviewId, Guid userId)
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

        review.Description = dto.Description;
        review.Rating = dto.Rating;

        await _reviewRepository.UpdateAsync(review);

        return new ResultDto(new { review.Id });
    }
}
