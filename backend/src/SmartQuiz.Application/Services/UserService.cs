using Microsoft.EntityFrameworkCore;
using SmartQuiz.Application.DTOs.Users;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class UserService : IUserService
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public UserService(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<GetUserDto?> GetUserDetailsAsync(Guid userId)
    {
        return await _userRepository.Query()
            .Include(x => x.Quizzes)
            .Include(x => x.Matches)
            .Select(x => new GetUserDto
            {
                Id = x.Id,
                Email = x.Email,
                Username = x.Username,
                MaxScore = x.Matches.Any() ? x.Matches.Max(m => m.Score) : 0,
                CreatedQuizzes = x.Quizzes.Count,
                MatchesPlayed = x.Matches.Count,
                TotalScore = x.Matches.Any() ? x.Matches.Sum(m => m.Score) : 0
            })
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public User CreateUser(string username, string email, string? password, bool isOAuthUser = false)
    {
        var passwordHash = password == null ? 
            string.Empty :
            _authService.HashPassword(password);
        
        return new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            IsOAuthUser = isOAuthUser
        };
    }

    public User UpdateUser(User user, UpdateUserDto dto)
    {
        user.Email = dto.Email;
        user.Username = dto.Username;

        return user;
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return _userRepository.GetByIdAsync(id);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }
    
    public async Task AddAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
    }
}