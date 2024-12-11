
using SmartQuiz.Core.Entities.Base;

namespace SmartQuiz.Core.Entities;

public class Question : Entity
{
    public string Text { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public List<AnswerOption> AnswerOptions { get; set; }
    public int Order { get; set; }
}
