using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Reviews;

public class DeleteReviewUseCase
{
    private readonly IReviewService _reviewService;
    private readonly IMatchService _matchService;
    private readonly IAuthService _authService;

    public DeleteReviewUseCase(IReviewService reviewService, IMatchService matchService, IAuthService authService)
    {
        _reviewService = reviewService;
        _matchService = matchService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid reviewId, Guid userId)
    {
        var review = await _reviewService.GetByIdAsync(reviewId);
        if (review == null) 
            throw new NotFoundException("Avaliação não encontrada");

        _authService.ValidateSameUser(review.UserId, userId);

        await _reviewService.DeleteAsync(review);
        
        _matchService.RemoveMatchReview(review.Match);
        
        await _matchService.UpdateAsync(review.Match);

        return new ResultDto(new { review.Id });
    }
}