
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories.Base;

namespace SmartQuiz.Core.Repositories;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<IEnumerable<Quiz>> SearchQuiz(string[]? keyWords, int skip, int take, Guid? userId);
    Task<bool> HasMatchesRelated(Guid quizId);
}
