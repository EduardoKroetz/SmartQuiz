namespace QuizDev.Application.DTOs.QuestionOptions;

public class GetQuestionOptionDto
{
    public GetQuestionOptionDto(Guid id, string response, Guid questionId)
    {
        Id = id;
        Response = response;
        QuestionId = questionId;
    }

    public Guid Id { get; set; }
    public string Response { get; set; }
    public Guid QuestionId { get; set; }
}
