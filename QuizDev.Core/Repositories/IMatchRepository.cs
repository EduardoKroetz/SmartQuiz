


using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IMatchRepository
{
    Task CreateAsync(Match match);
    Task<Question?> GetNextQuestion(Match match);
}
