using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Questions;

public class UpdateQuestionUseCase
{
    private readonly IQuestionService _questionService;
    private readonly IAuthService _authService;

    public UpdateQuestionUseCase(IQuestionService questionService, IAuthService authService)
    {
        _questionService = questionService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid questionId, UpdateQuestionDto dto, Guid userId)
    {
        var question = await _questionService.GetByIdAsync(questionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");

        _authService.ValidateSameUser(question.Quiz.UserId, userId);

        _questionService.UpdateQuestion(question, dto.Text);
        
        await _questionService.UpdateAsync(question);

        return new ResultDto(new { question.Id, question.Text });
    }
}