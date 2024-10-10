
using QuizDev.Core.Enums;

namespace QuizDev.Application.DTOs.Matches;

public class GetMatchDto
{
    public GetMatchDto(Guid id, int score, DateTime createdAt, EMatchStatus status, Guid quizId, Guid userId, bool reviewed, Guid? reviewId)
    {
        Id = id;
        Score = score;
        CreatedAt = createdAt;
        Status = status.ToString();
        QuizId = quizId;
        UserId = userId;
        Reviewed = reviewed;
        ReviewId = reviewId;
    }

    public Guid Id { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
    public bool Reviewed { get; set; }
    public Guid? ReviewId { get; set; }
}
