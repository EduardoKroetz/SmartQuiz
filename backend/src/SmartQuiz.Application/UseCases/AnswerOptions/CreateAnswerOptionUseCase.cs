using AutoMapper;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class CreateAnswerOptionUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public CreateAnswerOptionUseCase(IAnswerOptionRepository answerOptionRepository, IQuestionRepository questionRepository, IMapper mapper)
    {
        _answerOptionRepository = answerOptionRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(CreateAnswerOptionDto createAnswerOption, Guid userId)
    {
        var question = await _questionRepository.GetByIdAsync(createAnswerOption.QuestionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");

        if (question.Quiz.UserId != userId)
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");

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

        var answerOption = _mapper.Map<AnswerOption>(createAnswerOption);

        await _answerOptionRepository.AddAsync(answerOption);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}