using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.DTOs.Responses;

public class GetResponseDto
{
    public Guid QuestionId { get; set; }
    public Guid AnswerOptionId { get; set; }
    public GetAnswerOptionDto AnswerOption { get; set; }
    public GetAnswerOptionDto CorrectOption { get; set; }
    public bool IsCorrect { get; set; }
}