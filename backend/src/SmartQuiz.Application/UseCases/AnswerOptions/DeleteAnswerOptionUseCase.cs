using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class DeleteAnswerOptionUseCase
{
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IQuestionService _questionService;
    private readonly IAuthService _authService;

    public DeleteAnswerOptionUseCase(IAnswerOptionService answerOptionService, IQuestionService questionService, IAuthService authService)
    {
        _answerOptionService = answerOptionService;
        _questionService = questionService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid answerOptionId, Guid userId)
    {
        var answerOption = await _answerOptionService.GetByIdAsync(answerOptionId);
        if (answerOption == null) 
            throw new NotFoundException("Opção de resposta não encontrada");

        var question = await _questionService.GetByIdAsync(answerOption.QuestionId);
        if (question == null)
            throw new NotFoundException("Não foi possível encontrar a questão relacionada a opção de resposta");

        _authService.ValidateSameUser(question.Quiz.UserId, userId);
        
        await _answerOptionService.DeleteAsync(answerOption, question);

        return new ResultDto(new { AnswerOptionId = answerOption.Id, QuestionId = question.Id });
    }
}