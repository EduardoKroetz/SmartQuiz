using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly QuizDevDbContext _dbContext;

    public ReviewRepository(QuizDevDbContext dbContext)
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
        return await _dbContext.Reviews.Include(x => x.Match).FirstOrDefaultAsync(x => x.Id == reviewId);
    }
}
