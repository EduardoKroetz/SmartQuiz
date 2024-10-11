namespace QuizDev.Core.DTOs.AnswerOptions;

public class GetAnswerOptionDto
{
    public GetAnswerOptionDto(Guid id, string response, Guid questionId)
    {
        Id = id;
        Response = response;
        QuestionId = questionId;
    }

    public Guid Id { get; set; }
    public string Response { get; set; }
    public Guid QuestionId { get; set; }
}
