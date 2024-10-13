

using QuizDev.Core.Entities;

namespace QuizDev.Application.Services.Interfaces;

public interface IAuthService 
{
    string GenerateJwtToken(User user);
    bool VerifyPassword(string password, string passwordHash);
    string HashPassword(string password);
}
