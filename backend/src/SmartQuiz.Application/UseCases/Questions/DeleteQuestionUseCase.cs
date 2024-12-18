using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Questions;

public class DeleteQuestionUseCase
{
    private readonly IQuestionService _questionService;
    private readonly IQuizService _quizService;
    private readonly IAuthService _authService;

    public DeleteQuestionUseCase(IQuestionService questionService, IQuizService quizService, IAuthService authService)
    {
        _questionService = questionService;
        _quizService = quizService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid questionId, Guid userId)
    {
        var question = await _questionService.GetByIdAsync(questionId);
        if (question is null) 
            throw new NotFoundException("Questão não encontrada");
        
        _authService.ValidateSameUser(question.Quiz.UserId, userId);

        var quiz = await _quizService.GetByIdAsync(question.QuizId);
        if (quiz is null)
            throw new NotFoundException("Quiz não encontrado");
        
        _questionService.RemoveQuestionFromQuiz(quiz, question);
        
        await _questionService.DeleteAsync(question);

        return new ResultDto(new { question.Id });
    }
}