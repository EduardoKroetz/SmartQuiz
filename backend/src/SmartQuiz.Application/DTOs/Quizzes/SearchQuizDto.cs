namespace SmartQuiz.Application.DTOs.Quizzes;

public class SearchQuizDto
{
    public string? Reference { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public Guid? UserId { get; set; }
}
