using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories.Base;

namespace SmartQuiz.Core.Repositories;

public interface IAnswerOptionRepository : IRepository<AnswerOption>
{
    Task<List<AnswerOption>> GetByQuestionId(Guid questionId);
}
