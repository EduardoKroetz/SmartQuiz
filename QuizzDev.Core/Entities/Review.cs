

namespace QuizzDev.Core.Entities;

public class Review
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public Guid PlayId { get; set; }
    public Play Play { get; set; }
}
