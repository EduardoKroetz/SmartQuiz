using SmartQuiz.Core.Enums;

namespace SmartQuiz.Core.Entities;

public class Quiz
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Theme { get; set; }
    public EDifficulty Difficulty { get; set; }
    public bool Expires { get; set; }

    private int _expiresInSeconds { get; set; }
    public int ExpiresInSeconds
    {
        get
        {
            return _expiresInSeconds;
        }
        set
        {
            if (Expires)
            {
                ValidateExpiresInSeconds(value);
            }

            _expiresInSeconds = value;
        }
    }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public List<Question> Questions { get; set; }
    public List<Match> Matchs { get; set; }

    private void ValidateExpiresInSeconds(int expiresInSeconds)
    {
        if (expiresInSeconds < 10)
        {
            throw new ArgumentException("A expiração do Quiz deve ser maior que 10 segundos");
        }
    }

    public bool VerifyQuestionsSequenceOrder()
    {
        return Questions.Max(x => x.Order) == Questions.Count - 1;
    }

    public bool VerifyExistsOrder(int order)
    {
        return Questions.Any(x => x.Order.Equals(order));
    }

    public List<Question> GetQuestionsByOrderGratherThan(int order)
    {
        return Questions.Where(x => x.Order > order).ToList();
    }
}
