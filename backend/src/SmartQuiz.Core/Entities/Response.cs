

using SmartQuiz.Core.Entities.Base;

namespace SmartQuiz.Core.Entities;

public class Response : Entity
{
    public Guid AnswerOptionId { get; set; }
    public AnswerOption AnswerOption { get; set; }
    public Guid MatchId { get; set; }
    public Match Match { get; set; }
    public bool IsCorrect { get; set; }
}
