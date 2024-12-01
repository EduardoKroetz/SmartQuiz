using SmartQuiz.Core.Enums;

namespace SmartQuiz.Core.Entities;

public class Match
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public EMatchStatus Status { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime ExpiresIn
    {
        get
        {
            return CreatedAt.AddSeconds(Quiz.ExpiresInSeconds);
        }
    }
    public bool Reviewed { get; set; }
    public Guid? ReviewId { get; set; }
    public Review Review { get; set; }
    public List<Response> Responses { get; set; }

    public Response CreateResponse(AnswerOption option)
    {
        //Verificar se a questão da opção de resposta está entre as questões do quiz
        if (Quiz.Questions.Any(x => x.Id.Equals(option.QuestionId)) == false)
        {
            throw new InvalidOperationException("A opção de resposta não está disponível");
        }

        var response = new Response
        {
            Id = Guid.NewGuid(),
            MatchId = this.Id,
            IsCorrect = option.IsCorrectOption,
            AnswerOptionId = option.Id
        };

        Responses.Add(response);

        return response;
    }

    public void AddScore()
    {
        Score++;
    }

    public bool AlreadyExpiration()
    {
        return ExpiresIn > DateTime.UtcNow ? false : true;
    }


}
