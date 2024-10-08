using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IQuestionRepository
{
    Task CreateAsync(Question question);
    Task<Question?> GetAsync(Guid id, bool includeRelations = false);
    Task<Question?> GetQuizQuestionByOrder(Guid quizId, int order);
    Task UpdateRangeAsync(List<Question> questions);
}
