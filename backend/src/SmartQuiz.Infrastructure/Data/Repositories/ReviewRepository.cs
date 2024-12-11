using Microsoft.EntityFrameworkCore;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;
using SmartQuiz.Infrastructure.Data.Repositories.Base;

namespace SmartQuiz.Infrastructure.Data.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(SmartQuizDbContext dbContext) : base(dbContext)
    {
    }

    public new async Task<Review?> GetByIdAsync(Guid reviewId)
    {
        return await context.Reviews
            .Include(x => x.Match)
            .ThenInclude(x => x.Quiz)
            .FirstOrDefaultAsync(x => x.Id == reviewId);
    }
}