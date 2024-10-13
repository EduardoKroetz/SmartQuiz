
namespace QuizDev.Core.DTOs.Reviews;

public class GetReviewDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public Guid QuizId { get; set; }
    public Guid MatchId { get; set; }
    public Guid UserId { get; set; }
}
