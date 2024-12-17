using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IReviewService
{
    Task<Review?> GetByIdAsync(Guid id);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);

    Review CreateReview(CreateReviewDto dto, Match match, Guid userId);
    void UpdateReview(Review review, UpdateReviewDto dto);
}