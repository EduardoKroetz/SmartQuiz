using QuizDev.Core.Enums;

namespace QuizDev.Core.Entities;

public class Match
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public EMatchStatus Status { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public Guid UserId{ get; set; }
    public User User { get; set; }
    public bool Reviewed { get; set; }
    public Guid? ReviewId { get; set; }
    public Review Review { get; set; }
    public List<MatchResponse> Responses { get; private set; }

    public MatchResponse AddResponse(QuestionOption option)
    {
        var response = new MatchResponse
        {
            Id = Guid.NewGuid(),
            MatchId = this.Id,
            IsCorrect = option.IsCorrectOption,
            QuestionOptionId = option.Id
        };
       
        if (option.IsCorrectOption)
        {
            Score++;
        }

        Responses.Add(response);

        return response;
    }
}
