using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<GetUserDto?> GetUserDetailsAsync(Guid userId);
    
    User CreateUser(string username, string email, string? password, bool isOAuthUser = false);
    User UpdateUser(User user, UpdateUserDto dto);
}