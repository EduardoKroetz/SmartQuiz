
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IQuizRepository
{
    Task CreateAsync(Quiz quiz);
    Task<Quiz?> GetAsync(Guid id, bool includeQuestions = false);
}
