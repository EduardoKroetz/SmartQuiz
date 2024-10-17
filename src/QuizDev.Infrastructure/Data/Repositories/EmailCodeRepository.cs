
using Microsoft.EntityFrameworkCore;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Infrastructure.Data.Repositories;

public class EmailCodeRepository : IEmailCodeRepository
{
    private readonly QuizDevDbContext _dbContext;

    public EmailCodeRepository(QuizDevDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(EmailCode emailCode)
    {
        await _dbContext.EmailCodes.AddAsync(emailCode);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<EmailCode?> GetByEmailAsync(string email)
    {
        return await _dbContext.EmailCodes.FirstOrDefaultAsync(x => x.Email.Equals(email));
    }

    public async Task<EmailCode?> GetByCodeAsync(string code)
    {
        return await _dbContext.EmailCodes.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task DeleteAsync(EmailCode emailCode)
    {
        _dbContext.EmailCodes.Remove(emailCode);
        await _dbContext.SaveChangesAsync();
    }
}
