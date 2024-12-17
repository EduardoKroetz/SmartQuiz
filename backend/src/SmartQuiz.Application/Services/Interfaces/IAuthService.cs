using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    bool VerifyPassword(string password, string passwordHash);
    string HashPassword(string password);
    void ValidateSameUser(Guid userId, Guid authUserId);
}