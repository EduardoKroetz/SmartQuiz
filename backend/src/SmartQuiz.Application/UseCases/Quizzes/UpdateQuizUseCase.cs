
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class UpdateQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public UpdateQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(Guid quizId, EditorQuizDto editorQuizDto, Guid userId)
    {
        var quiz = await _quizRepository.GetAsync(quizId);
        if (quiz == null)
        {
            throw new NotFoundException("Quiz não encontrado");
        }

        if (quiz.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não possui permissão para acessar esse recurso");
        }

        quiz.Title = editorQuizDto.Title;
        quiz.Description = editorQuizDto.Description;
        quiz.Expires = editorQuizDto.Expires;
        quiz.ExpiresInSeconds = editorQuizDto.ExpiresInSeconds;

        await _quizRepository.UpdateAsync(quiz);

        return new ResultDto(new { quiz.Id });
    }
}
