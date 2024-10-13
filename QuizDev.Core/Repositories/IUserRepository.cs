using QuizDev.Core.DTOs.Matches;
using QuizDev.Core.DTOs.Quizzes;
using QuizDev.Core.DTOs.Users;
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<GetUserDto?> GetDetailsAsync(Guid userId);
    Task<List<GetMatchDto>> GetUserMatchesAsync(Guid userId, int skip, int take);
    Task<List<GetQuizDto>> GetUserQuizzesAsync(Guid userId, int skip, int take);
}
