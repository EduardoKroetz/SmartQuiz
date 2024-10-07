using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IQuestionOptionRepository
{
    Task<QuestionOption?> GetById(Guid id);
    Task CreateAsync(QuestionOption option);
}
