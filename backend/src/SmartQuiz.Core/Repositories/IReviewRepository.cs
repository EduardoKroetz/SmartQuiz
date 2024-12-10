
using SmartQuiz.Core.DTOs.Reviews;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetById(Guid reviewId);
    Task CreateAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
    Task<GetReviewDto?> GetDetailsAsync(Guid reviewId);
}
