namespace QuizDev.Application.DTOs.QuestionOptions;

public class GetQuestionOptionDto
{
    public GetQuestionOptionDto(Guid id, string response, bool isCorrectOption, Guid questionId)
    {
        Id = id;
        Response = response;
        IsCorrectOption = isCorrectOption;
        QuestionId = questionId;
    }

    public Guid Id { get; set; }
    public string Response { get; set; }
    public bool IsCorrectOption { get; set; }
    public Guid QuestionId { get; set; }
}
