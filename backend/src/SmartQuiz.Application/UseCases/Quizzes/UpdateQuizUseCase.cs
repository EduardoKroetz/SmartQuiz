using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class UpdateQuizUseCase
{
    private readonly IQuizService _quizService;
    private readonly IAuthService _authService;

    public UpdateQuizUseCase(IQuizService quizService, IAuthService authService)
    {
        _quizService = quizService;
        _authService = authService;
    }
    
    public async Task<ResultDto> Execute(Guid quizId, EditorQuizDto editorQuizDto, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId);
        if (quiz == null) 
            throw new NotFoundException("Quiz não encontrado");

        _authService.ValidateSameUser(quiz.UserId, userId);
        
        _quizService.UpdateQuiz(quiz, editorQuizDto);

        await _quizService.UpdateAsync(quiz);

        return new ResultDto(new { quiz.Id });
    }
}