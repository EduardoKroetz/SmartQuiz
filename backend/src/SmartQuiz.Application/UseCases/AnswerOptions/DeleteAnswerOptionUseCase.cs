using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Application.Validators.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class DeleteAnswerOptionUseCase
{
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IQuestionService _questionService;
    private readonly IUserAuthorizationValidator _userAuthorizationValidator;

    public DeleteAnswerOptionUseCase(IAnswerOptionService answerOptionService, IQuestionService questionService, IUserAuthorizationValidator userAuthorizationValidator)
    {
        _answerOptionService = answerOptionService;
        _questionService = questionService;
        _userAuthorizationValidator = userAuthorizationValidator;
    }

    public async Task<ResultDto> Execute(Guid answerOptionId, Guid userId)
    {
        var answerOption = await _answerOptionService.GetByIdAsync(answerOptionId);
        if (answerOption == null) 
            throw new NotFoundException("Opção de resposta não encontrada");

        var question = await _questionService.GetByIdAsync(answerOption.QuestionId);
        if (question == null)
            throw new NotFoundException("Não foi possível encontrar a questão relacionada a opção de resposta");

        _userAuthorizationValidator.ValidateAuthorization(question.Quiz.UserId, userId);
        
        await _answerOptionService.DeleteAsync(answerOption, question);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}