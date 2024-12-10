using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Core.Repositories;

public interface IAnswerOptionRepository
{
    Task<AnswerOption?> GetById(Guid id);
    Task CreateAsync(AnswerOption option);
    Task UpdateAsync(AnswerOption option);
    Task DeleteAsync(AnswerOption option);
    Task<List<GetAnswerOptionDto>> GetByQuestionId(Guid questionId);
}
