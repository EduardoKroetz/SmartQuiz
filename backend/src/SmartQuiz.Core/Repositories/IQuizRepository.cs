
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories.Base;

namespace SmartQuiz.Core.Repositories;

public interface IQuizRepository : IRepository<Quiz>
{
    Task<List<Quiz>> SearchQuiz(string[]? keyWords, int skip, int take);
    Task<bool> HasMatchesRelated(Guid quizId);
    Task<List<Quiz>> GetUserQuizzesAsync(Guid userId, int skip, int take);
}
