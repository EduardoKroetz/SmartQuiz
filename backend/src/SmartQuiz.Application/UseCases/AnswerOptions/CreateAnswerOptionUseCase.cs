using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class CreateAnswerOptionUseCase
{
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IQuestionService _questionService;
    private readonly IAuthService _authService;
    
    public CreateAnswerOptionUseCase(IAnswerOptionService answerOptionService, IQuestionService questionService, IAuthService authService)
    {
        _answerOptionService = answerOptionService;
        _questionService = questionService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(CreateAnswerOptionDto createAnswerOption, Guid userId)
    {
        // Buscar a questão
        var question = await _questionService.GetByIdAsync(createAnswerOption.QuestionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");
        
        // Validar se quem está criando é quem está autenticado
        _authService.ValidateSameUser(question.Quiz.UserId, userId);

        //Caso seja a opção que está sendo criada é a correta da questão, vai remover a opção correta atual da questão
        if (createAnswerOption.IsCorrectOption)
            await _answerOptionService.UpdateCorrectOption(question);
    
        // Criar nova instância
        var answerOption = _answerOptionService.CreateAnswerOption(createAnswerOption);

        // Salvar no banco de dados
        await _answerOptionService.AddAsync(answerOption);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}