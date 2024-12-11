using SmartQuiz.Core.Enums;

namespace SmartQuiz.Application.DTOs.Quizzes;

public class GetQuizDto
{
    public GetQuizDto(Guid id, string title, string description, bool expires, int expiresInSeconds, bool isActive,
        string theme, int numberOfQuestion, EDifficulty difficulty, Guid userId)
    {
        Id = id;
        Title = title;
        Description = description;
        Expires = expires;
        ExpiresInSeconds = expiresInSeconds;
        IsActive = isActive;
        Theme = theme;
        NumberOfQuestion = numberOfQuestion;
        Difficulty = difficulty.ToString();
        UserId = userId;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
    public bool IsActive { get; set; }
    public string Theme { get; set; }
    public int NumberOfQuestion { get; set; }
    public string Difficulty { get; set; }
    public Guid UserId { get; set; }
}