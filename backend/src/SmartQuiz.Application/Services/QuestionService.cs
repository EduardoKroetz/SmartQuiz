using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Question?> GetByIdAsync(Guid questionId)
    {
        return await _questionRepository.GetByIdAsync(questionId);
    }
}