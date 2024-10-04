namespace QuizDev.Core.Entities;

public class Play
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public Guid UserId{ get; set; }
    public User User { get; set; }
    public bool Reviewed { get; set; }
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }
}
