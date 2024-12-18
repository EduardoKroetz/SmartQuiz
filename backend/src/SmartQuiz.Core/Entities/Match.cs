using SmartQuiz.Core.Entities.Base;
using SmartQuiz.Core.Enums;

namespace SmartQuiz.Core.Entities;

public class Match : Entity
{
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public EMatchStatus Status { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime ExpiresIn
    {
        get
        {
            return CreatedAt.AddSeconds(Quiz.ExpiresInSeconds);
        }
    }
    public bool Reviewed { get; set; }
    public Guid? ReviewId { get; set; }
    public Review Review { get; set; }
    public List<Response> Responses { get; set; }

    public int GetRemainingTime()
    {
        var totalSeconds = (int)(ExpiresIn - DateTime.UtcNow).TotalSeconds;
        return totalSeconds < 0 ? 0 : totalSeconds;
    }
}
