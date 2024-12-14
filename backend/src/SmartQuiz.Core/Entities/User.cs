using SmartQuiz.Core.Entities.Base;

namespace SmartQuiz.Core.Entities;

public class User : Entity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsOAuthUser { get; set; } = false;
    public List<Match> Matches { get; set; } = [];
    public List<Quiz> Quizzes { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}
