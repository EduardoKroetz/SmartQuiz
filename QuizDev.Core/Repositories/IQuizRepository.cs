
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IQuizRepository
{
    Task CreateAsync(Quiz quiz);
    Task<Quiz?> GetAsync(Guid id, bool includeQuestions = false);
    Task<List<Quiz>> SearchQuizByReviews(string[] keyWords, int skip, int take);
    Task<List<Quiz>> SearchQuiz(string[] keyWords, int skip, int take);
    Task UpdateAsync(Quiz quiz);
}
