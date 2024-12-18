using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class DeleteQuizUseCase
{
    private readonly IQuizService _quizService;
    private readonly IAuthService _authService;

    public DeleteQuizUseCase(IQuizService quizService, IAuthService authService)
    {
        _quizService = quizService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid quizId, Guid userId)
    {
        if (await _quizService.HasMatchesRelated(quizId))
            throw new InvalidOperationException("Você pode somente desativar o Quiz nesse momento");

        var quiz = await _quizService.GetByIdAsync(quizId);
        if (quiz == null) 
            throw new NotFoundException("Quiz não encontrado");

        _authService.ValidateSameUser(quiz.UserId, userId);

        await _quizService.DeleteAsync(quiz);

        return new ResultDto(new { quiz.Id });
    }
}