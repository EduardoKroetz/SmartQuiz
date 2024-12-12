using SmartQuiz.Application.DTOs.AnswerOptions;

namespace SmartQuiz.Application.DTOs.Questions;

public class GetQuestionWithCorrectOptionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid QuizId { get; set; }
    public int Order { get; set; }
    public GetAnswerOptionDto CorrectOption { get; set; }
    public List<GetAnswerOptionDto> Options { get; set; }
}