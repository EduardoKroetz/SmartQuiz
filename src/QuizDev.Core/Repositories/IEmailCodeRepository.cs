
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IEmailCodeRepository
{
    Task CreateAsync(EmailCode emailCode);
    Task<EmailCode?> GetByEmailAsync(string email);
    Task DeleteAsync(EmailCode emailCode);
}
