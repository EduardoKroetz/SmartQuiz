using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Enums;
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
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null) 
            throw new NotFoundException("Quiz não encontrado");

        if (quiz.UserId != userId)
            throw new UnauthorizedAccessException("Você não possui permissão para acessar esse recurso");
        
        if (!Enum.TryParse(editorQuizDto.Difficulty, true, out EDifficulty difficulty))
            throw new InvalidOperationException("Dificuldade inválida. Dificuldades disponíveis: easy, medium, hard");

        quiz.Title = editorQuizDto.Title;
        quiz.Description = editorQuizDto.Description;
        quiz.Expires = editorQuizDto.Expires;
        quiz.ExpiresInSeconds = editorQuizDto.ExpiresInSeconds;
        quiz.Difficulty = difficulty;
        quiz.Theme = editorQuizDto.Theme;

        await _quizRepository.UpdateAsync(quiz);

        return new ResultDto(new { quiz.Id });
    }
}