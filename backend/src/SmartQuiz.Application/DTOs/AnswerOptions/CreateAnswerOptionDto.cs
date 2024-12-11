namespace SmartQuiz.Application.DTOs.AnswerOptions;

public class CreateAnswerOptionDto
{
    public string Response { get; set; }
    public bool IsCorrectOption { get; set; }
    public Guid QuestionId { get; set; }
}