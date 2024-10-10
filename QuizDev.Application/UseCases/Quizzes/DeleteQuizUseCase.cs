
using QuizDev.Application.DTOs.Responses;
using QuizDev.Application.Exceptions;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

public class DeleteQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public DeleteQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(Guid quizId, Guid userId)
    {
        if (await _quizRepository.HasMatchesRelated(quizId))
        {
            throw new InvalidOperationException("Você pode somente desativar o Quiz nesse momento");
        }

        var quiz = await _quizRepository.GetAsync(quizId);
        if (quiz == null)
        {
            throw new NotFoundException("Quiz não encontrado");
        }
        
        if (quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
        }

        await _quizRepository.DeleteAsync(quiz);

        return new ResultDto(new { quiz.Id });
    }
}
    