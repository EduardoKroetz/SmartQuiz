using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.Validators.Interfaces;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class CreateAnswerOptionUseCase
{
    private readonly IUserAuthorizationValidator _userAuthorizationValidator;
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IQuestionService _questionService;

    public CreateAnswerOptionUseCase(IUserAuthorizationValidator userAuthorizationValidator, IAnswerOptionService answerOptionService, IQuestionService questionService)
    {
        _userAuthorizationValidator = userAuthorizationValidator;
        _answerOptionService = answerOptionService;
        _questionService = questionService;
    }

    public async Task<ResultDto> Execute(CreateAnswerOptionDto createAnswerOption, Guid userId)
    {
        // Buscar a questão
        var question = await _questionService.GetByIdAsync(createAnswerOption.QuestionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");
        
        // Validar se quem está criando é quem está autenticado
        _userAuthorizationValidator.ValidateAuthorization(question.Quiz.UserId, userId);

        //Caso seja a opção que está sendo criada é a correta da questão, vai remover a opção correta atual da questão
        if (createAnswerOption.IsCorrectOption)
            await _answerOptionService.UpdateCorrectOption(question);
    
        // Criar nova instância
        var answerOption = _answerOptionService.CreateAnswerOption(createAnswerOption);

        // Salvar no banco de dados
        await _answerOptionService.SaveAsync(answerOption);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}