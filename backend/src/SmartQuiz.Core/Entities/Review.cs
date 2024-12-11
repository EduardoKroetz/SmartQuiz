

using SmartQuiz.Core.Entities.Base;

namespace SmartQuiz.Core.Entities;

public class Review : Entity
{
    public string Description { get; set; }
    private int _rating { get; set; }
    public int Rating
    {
        get
        {
            return _rating;
        }
        set
        {
            if (value < 0 || value > 10)
            {
                throw new ArgumentException("A nota deve ser entre 0 e 10");
            }

            _rating = value;
        }
    }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public Guid MatchId { get; set; }
    public Match Match { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
