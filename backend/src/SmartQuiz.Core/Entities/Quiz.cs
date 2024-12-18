using SmartQuiz.Core.Entities.Base;
using SmartQuiz.Core.Enums;

namespace SmartQuiz.Core.Entities;

public class Quiz : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Theme { get; set; }
    public EDifficulty Difficulty { get; set; }
    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public List<Question> Questions { get; set; } = [];
    public List<Match> Matchs { get; set; } = [];
}
