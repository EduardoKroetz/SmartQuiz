
using SmartQuiz.Core.DTOs.AnswerOptions;

namespace SmartQuiz.Core.DTOs.Questions;

public class GetQuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid QuizId { get; set; }
    public int Order { get; set; }
    public List<GetAnswerOptionDto> Options { get; set; }
}
