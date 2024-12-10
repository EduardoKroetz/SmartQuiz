using SmartQuiz.Core.DTOs.Matches;
using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.DTOs.Users;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByEmailAsync(string email);
    Task<GetUserDto?> GetDetailsAsync(Guid userId);
    Task<List<GetMatchDto>> GetUserMatchesAsync(Guid userId, int skip, int take);
    Task<List<GetQuizDto>> GetUserQuizzesAsync(Guid userId, int skip, int take);
    Task UpdateAsync(User user);
}
