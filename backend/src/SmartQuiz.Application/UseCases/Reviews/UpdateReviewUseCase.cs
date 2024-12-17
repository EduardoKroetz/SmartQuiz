using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Reviews;

public class UpdateReviewUseCase
{
    private readonly IReviewService _reviewService;
    private readonly IAuthService _authService;

    public UpdateReviewUseCase(IReviewService reviewService, IAuthService authService)
    {
        _reviewService = reviewService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(UpdateReviewDto dto, Guid reviewId, Guid userId)
    {
        var review = await _reviewService.GetByIdAsync(reviewId);
        if (review == null) 
            throw new NotFoundException("Avaliação não encontrada");

        _authService.ValidateSameUser(review.UserId, userId);

        _reviewService.UpdateReview(review, dto);
        
        await _reviewService.UpdateAsync(review);

        return new ResultDto(new { review.Id });
    }
}