
namespace QuizDev.Core.Entities;

public class QuestionOption
{
    public Guid Id { get; set; }
    public string Response { get; set; }
    public bool IsCorrectOption { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}
