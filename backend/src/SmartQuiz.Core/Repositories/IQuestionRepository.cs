using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories.Base;

namespace SmartQuiz.Core.Repositories;

public interface IQuestionRepository : IRepository<Question>
{
    Task<Question?> GetQuizQuestionByOrder(Guid quizId, int order);
    Task UpdateRangeAsync(List<Question> questions);
    Task<List<Question>> GetQuestionsByQuizId(Guid quizId);
}
