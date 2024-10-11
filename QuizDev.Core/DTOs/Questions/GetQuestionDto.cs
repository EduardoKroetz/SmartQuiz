
using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.Entities;

namespace QuizDev.Core.DTOs.Questions;

public class GetQuestionDto
{
    public GetQuestionDto(Guid id, string text, Guid quizId, int order, List<AnswerOption> options)
    {
        Id = id;
        Text = text;
        QuizId = quizId;
        Order = order;
        Options = options.Select(x => new GetAnswerOptionDto(x.Id, x.Response, x.QuestionId)).ToList();
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid QuizId { get; set; }
    public int Order { get; set; }
    public List<GetAnswerOptionDto> Options { get; set; }
}
