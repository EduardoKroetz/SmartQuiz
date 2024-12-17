using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Questions;

public class UpdateCorrectOptionUseCase
{
    private readonly IQuestionService _questionService;
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IAuthService _authService;

    public UpdateCorrectOptionUseCase(IQuestionService questionService, IAnswerOptionService answerOptionService, IAuthService authService)
    {
        _questionService = questionService;
        _answerOptionService = answerOptionService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid questionId, Guid newCorrectOptionId, Guid userId)
    {
        var question = await _questionService.GetByIdAsync(questionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");

        _authService.ValidateSameUser(question.Quiz.UserId, userId);

        _questionService.UpdateCorrectOption(question, newCorrectOptionId);
        
        await _questionService.UpdateAsync(question);

        return new ResultDto(new { QuestionId = questionId, AnswerOptionId = newCorrectOptionId });
    }
}