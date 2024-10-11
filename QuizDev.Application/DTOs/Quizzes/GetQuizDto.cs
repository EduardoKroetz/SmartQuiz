
namespace QuizDev.Application.DTOs.Quizzes;

public class GetQuizDto
{
    public GetQuizDto(Guid id, string title, string description, bool expires, int expiresInSeconds, bool isActive, Guid userId)
    {
        Id = id;
        Title = title;
        Description = description;
        Expires = expires;
        ExpiresInSeconds = expiresInSeconds;
        IsActive = isActive;
        UserId = userId;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
}
