using QuizDev.Application.DTOs.Questions;
using QuizDev.Core.Entities;

namespace QuizDev.Application.DTOs.Quizzes;

public class GetQuizDto
{
    public GetQuizDto(Guid id, string title, string description, bool expires, int expiresInSeconds, bool isActive, Guid userId, List<Question> questions)
    {
        Id = id;
        Title = title;
        Description = description;
        Expires = expires;
        ExpiresInSeconds = expiresInSeconds;
        IsActive = isActive;
        UserId = userId;
        Questions = questions.Select(x => new GetQuestionDto(x.Id, x.Text, x.QuizId, x.Order ,x.Options)).ToList();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public List<GetQuestionDto> Questions { get; set; }
}
