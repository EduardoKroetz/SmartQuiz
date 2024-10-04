
namespace QuizzDev.Core.Entities;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid CorrectAnswer { get; set; }
    public List<QuestionOption> Options { get; set; }
}
