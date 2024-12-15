using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IAnswerOptionService
{
    Task UpdateCorrectOption(Question question);
    Task<AnswerOption?> GetByIdAsync(Guid id);
    Task SaveAsync(AnswerOption answerOption);
    AnswerOption CreateAnswerOption(CreateAnswerOptionDto dto);
    Task DeleteAsync(AnswerOption answerOption, Question question);
    Task UpdateAsync(AnswerOption answerOption);
}