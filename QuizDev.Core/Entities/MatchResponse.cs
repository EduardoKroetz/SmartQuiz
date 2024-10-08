

namespace QuizDev.Core.Entities;

public class MatchResponse
{
    public Guid Id { get; set; }
    public Guid QuestionOptionId { get; set; }
    public QuestionOption QuestionOption { get; set; }
    public Guid MatchId { get; set; }
    public Match Match { get; set; }
    public bool IsCorrect { get; set; }
}
