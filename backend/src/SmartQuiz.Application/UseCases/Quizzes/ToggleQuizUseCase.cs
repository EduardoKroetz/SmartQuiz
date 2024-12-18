using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class ToggleQuizUseCase
{
    private readonly IAuthService _authService;
    private readonly IQuizService _quizService;

    public ToggleQuizUseCase(IQuizService quizService, IAuthService authService)
    {
        _quizService = quizService;
        _authService = authService;
    }
    
    public async Task<ResultDto> Execute(Guid quizId, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId);
        if (quiz == null) 
            throw new NotFoundException("Quiz não encontrado");

        _authService.ValidateSameUser(quiz.UserId, userId);

        _quizService.ToggleQuiz(quiz);

        await _quizService.UpdateAsync(quiz);

        return new ResultDto(new { QuizId = quizId, quiz.IsActive });
    }
}