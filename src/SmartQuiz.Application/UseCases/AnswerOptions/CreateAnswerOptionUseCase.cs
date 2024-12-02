
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class CreateAnswerOptionUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;
    private readonly IQuestionRepository _questionRepository;

    public CreateAnswerOptionUseCase(IAnswerOptionRepository answerOptionRepository, IQuestionRepository questionRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(CreateAnswerOptionDto createAnswerOption, Guid userId)
    {
        var question = await _questionRepository.GetAsync(createAnswerOption.QuestionId);
        if (question == null)
        {
            throw new NotFoundException("Questão não encontrada");
        }

        if (question.Quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        //Caso seja a opção que está sendo criada é a correta da questão, vai remover a opção correta atual da questão
        if (createAnswerOption.IsCorrectOption)
        {
            var correctOption = question.AnswerOptions.FirstOrDefault(x => x.IsCorrectOption);

            if (correctOption != null)
            {
                correctOption.IsCorrectOption = false;
                await _answerOptionRepository.UpdateAsync(correctOption);
            }
        }

        var answerOption = new AnswerOption
        {
            Id = Guid.NewGuid(),
            IsCorrectOption = createAnswerOption.IsCorrectOption,
            QuestionId = createAnswerOption.QuestionId,
            Response = createAnswerOption.Response
        };

        await _answerOptionRepository.CreateAsync(answerOption);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}
