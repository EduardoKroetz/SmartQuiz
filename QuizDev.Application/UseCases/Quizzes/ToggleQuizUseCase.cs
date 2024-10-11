using QuizDev.Application.DTOs.Responses;
using QuizDev.Application.Exceptions;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

public class ToggleQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public ToggleQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(Guid quizId, Guid userId)
    {
        var quiz = await _quizRepository.GetAsync(quizId, true);
        if (quiz == null)
        {
            throw new NotFoundException("Quiz não encontrado");
        }

        if (quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        if (quiz.IsActive == false)
        {
            if (quiz.Questions.Count == 0)
            {
                throw new ArgumentException("Não é possível ativar o Quiz pois ele não possui nenhuma questão relacionada");
            }
        }

        quiz.IsActive = !quiz.IsActive;

        await _quizRepository.UpdateAsync(quiz);

        return new ResultDto(new { QuizId = quizId, quiz.IsActive });
    }
}
