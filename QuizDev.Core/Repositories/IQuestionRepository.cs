using QuizDev.Core.DTOs.Questions;
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IQuestionRepository
{
    Task CreateAsync(Question question);
    Task<Question?> GetAsync(Guid id, bool includeRelations = true);
    Task<Question?> GetQuizQuestionByOrder(Guid quizId, int order);
    Task UpdateRangeAsync(List<Question> questions);
    Task<List<GetQuestionDto>> GetQuestionsByQuizId(Guid quizId);
    Task<GetQuestionDto?> GetQuestionDetails(Guid questionId);
    Task UpdateAsync(Question question);
}
