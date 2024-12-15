using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IQuestionService
{
    Task<Question?> GetByIdAsync(Guid questionId);
}