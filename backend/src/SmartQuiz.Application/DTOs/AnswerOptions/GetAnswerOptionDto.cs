namespace SmartQuiz.Application.DTOs.AnswerOptions;

public class GetAnswerOptionDto
{
    public GetAnswerOptionDto(Guid id, string response)
    {
        Id = id;
        Response = response;
    }

    public Guid Id { get; set; }
    public string Response { get; set; }
}