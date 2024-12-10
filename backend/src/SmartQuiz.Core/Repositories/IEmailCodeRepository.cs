
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.Repositories;

public interface IEmailCodeRepository
{
    Task CreateAsync(EmailCode emailCode);
    Task<EmailCode?> GetByEmailAsync(string email);
    Task DeleteAsync(EmailCode emailCode);
    Task<EmailCode?> GetByCodeAsync(string code);
}
