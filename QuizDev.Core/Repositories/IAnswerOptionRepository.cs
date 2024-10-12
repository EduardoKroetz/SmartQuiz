using QuizDev.Core.DTOs.AnswerOptions;
using QuizDev.Core.Entities;

namespace QuizDev.Core.Repositories;

public interface IAnswerOptionRepository
{
    Task<AnswerOption?> GetById(Guid id);
    Task CreateAsync(AnswerOption option);
    Task UpdateAsync(AnswerOption option);
    Task DeleteAsync(AnswerOption option);
    Task<List<GetAnswerOptionDto>> GetByQuestionId(Guid questionId);
}
