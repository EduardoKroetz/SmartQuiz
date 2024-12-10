
using SmartQuiz.Core.DTOs.Quizzes;

namespace SmartQuiz.Core.DTOs.Matches;

public class GetMatchDto
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresIn { get; set; }
    public string Status { get; set; }
    public Guid QuizId { get; set; }
    public GetQuizDto Quiz { get; set; }
    public Guid UserId { get; set; }
    public bool Reviewed { get; set; }
    public Guid? ReviewId { get; set; }
    public int RemainingTimeInSeconds { get; set; }
}
