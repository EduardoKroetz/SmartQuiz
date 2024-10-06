using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
