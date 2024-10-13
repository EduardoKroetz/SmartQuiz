
namespace QuizDev.Core.DTOs.Reviews;

public class CreateReviewDto
{
    public string Description { get; set; }
    public int Rating { get; set; }
    public Guid MatchId { get; set; }
}
