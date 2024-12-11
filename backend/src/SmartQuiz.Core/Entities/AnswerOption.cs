
using SmartQuiz.Core.Entities.Base;

namespace SmartQuiz.Core.Entities;

public class AnswerOption : Entity
{
    public string Response { get; set; }
    public bool IsCorrectOption { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}
