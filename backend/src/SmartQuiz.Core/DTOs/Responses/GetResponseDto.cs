using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.DTOs.Responses;

public class GetResponseDto
{
    public GetResponseDto(Guid questionId, Guid answerOptionId, AnswerOption answerOption, AnswerOption correctAnswerOption, bool isCorrect)
    {
        QuestionId = questionId;
        AnswerOptionId = answerOptionId;
        AnswerOption = new GetAnswerOptionDto(answerOption.Id, answerOption.Response);;
        CorrectOption = new GetAnswerOptionDto(correctAnswerOption.Id, correctAnswerOption.Response);
        IsCorrect = isCorrect;
    }

    public Guid QuestionId { get; set; }
    public Guid AnswerOptionId { get; set; }
    public GetAnswerOptionDto AnswerOption { get; set; }
    public GetAnswerOptionDto CorrectOption { get; set; }
    public bool IsCorrect { get; set; }
}
