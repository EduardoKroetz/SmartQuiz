namespace QuizDev.Core.Entities;

public class Quiz
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public List<Question> Questions { get; set; }
    public List<Match> Matchs { get; set; }

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
