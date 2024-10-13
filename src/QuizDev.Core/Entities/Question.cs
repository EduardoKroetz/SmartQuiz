
namespace QuizDev.Core.Entities;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public List<AnswerOption> Options { get; set; }
    public int Order { get; set; }
}
