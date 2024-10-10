

namespace QuizDev.Core.Entities;

public class Response
{
    public Guid Id { get; set; }
    public Guid AnswerOptionId { get; set; }
    public AnswerOption AnswerOption { get; set; }
    public Guid MatchId { get; set; }
    public Match Match { get; set; }
    public bool IsCorrect { get; set; }
}
