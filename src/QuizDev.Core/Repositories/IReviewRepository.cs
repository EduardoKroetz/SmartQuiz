
using QuizDev.Core.DTOs.Reviews;
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetById(Guid reviewId);
    Task CreateAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
    Task<GetReviewDto?> GetDetailsAsync(Guid reviewId);
}
