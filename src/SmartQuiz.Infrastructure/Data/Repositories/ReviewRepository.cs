using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.DTOs.Reviews;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly SmartQuizDbContext _dbContext;

    public ReviewRepository(SmartQuizDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Review review)
    {
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
    }


    public async Task UpdateAsync(Review review)
    {
        _dbContext.Reviews.Update(review);
        await _dbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(Review review)
    {
        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Review?> GetById(Guid reviewId)
    {
        return await _dbContext.Reviews
            .Include(x => x.Match)
            .ThenInclude(x => x.Quiz)
            .FirstOrDefaultAsync(x => x.Id == reviewId);
    }

    public async Task<GetReviewDto?> GetDetailsAsync(Guid reviewId)
    {
        return await _dbContext.Reviews
            .Select(x => new GetReviewDto
            {
                Id = x.Id,
                Description = x.Description,
                MatchId = x.MatchId,
                QuizId = x.QuizId,
                Rating = x.Rating,
                UserId = x.UserId
            })
            .FirstOrDefaultAsync(x => x.Id == reviewId);
    }
}
