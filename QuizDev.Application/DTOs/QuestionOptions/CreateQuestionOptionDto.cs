
namespace QuizDev.Application.DTOs.QuestionOptions;

public class CreateQuestionOptionDto
{
    public string Response { get; set; }
    public bool IsCorrectOption { get; set; }
    public Guid QuestionId { get; set; }
}
