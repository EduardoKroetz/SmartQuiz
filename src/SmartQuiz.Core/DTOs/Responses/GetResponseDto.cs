using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.DTOs.Responses;

public class GetResponseDto
{
    public GetResponseDto(Guid id, Guid answerOptionId, AnswerOption answerOption, Guid matchId, bool isCorrect)
    {
        Id = id;
        AnswerOptionId = answerOptionId;
        AnswerOption = new GetAnswerOptionDto(answerOption.Id, answerOption.Response, answerOption.QuestionId);
        MatchId = matchId;
        IsCorrect = isCorrect;
    }

    public Guid Id { get; set; }
    public Guid AnswerOptionId { get; set; }
    public GetAnswerOptionDto AnswerOption { get; set; }
    public Guid MatchId { get; set; }
    public bool IsCorrect { get; set; }
}
