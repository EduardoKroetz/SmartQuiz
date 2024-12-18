using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class AnswerOptionService : IAnswerOptionService
{
    private readonly IAnswerOptionRepository _answerOptionRepository;

    public AnswerOptionService(IAnswerOptionRepository answerOptionRepository)
    {
        _answerOptionRepository = answerOptionRepository;
    }

    public async Task UpdateCorrectOption(Question question)
    {
        var correctOption = question.AnswerOptions.FirstOrDefault(x => x.IsCorrectOption);

        if (correctOption != null)
        {
            correctOption.IsCorrectOption = false;
            await _answerOptionRepository.UpdateAsync(correctOption);
        }
    }

    public async Task<AnswerOption?> GetByIdAsync(Guid id)
    {
        return await _answerOptionRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(AnswerOption answerOption)
    {
        await _answerOptionRepository.AddAsync(answerOption);
    }

    public AnswerOption CreateAnswerOption(CreateAnswerOptionDto dto)
    {
        return new AnswerOption()
        {
            Response = dto.Response,
            IsCorrectOption = dto.IsCorrectOption,
            QuestionId = dto.QuestionId,
        };
    }

    public async Task DeleteAsync(AnswerOption answerOption, Question question)
    {
        if (question.AnswerOptions.Count <= 2)
            throw new InvalidOperationException("Não é possível deletar a opção de resposta pois a Questão relacionada deve possuir no mínimo duas opções de resposta");
        
        await _answerOptionRepository.DeleteAsync(answerOption);
    }

    public async Task UpdateAsync(AnswerOption answerOption)
    {
        await _answerOptionRepository.UpdateAsync(answerOption);
    }
}