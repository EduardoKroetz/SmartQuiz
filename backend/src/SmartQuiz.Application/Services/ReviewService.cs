using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }


    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _reviewRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Review review)
    { 
        await _reviewRepository.AddAsync(review);
    }

    public async Task UpdateAsync(Review review)
    { 
        await _reviewRepository.UpdateAsync(review);
    }

    public async Task DeleteAsync(Review review)
    {
        await _reviewRepository.DeleteAsync(review);
    }

    public Review CreateReview(CreateReviewDto dto, Match match, Guid userId)
    {
        return new Review
        {
            Description = dto.Description,
            Rating = dto.Rating,
            MatchId = dto.MatchId,
            QuizId = match.QuizId,
            UserId = userId
        };
    }

    public void UpdateReview(Review review, UpdateReviewDto dto)
    {
        review.Description = dto.Description;
        review.Rating = dto.Rating;
    }
}