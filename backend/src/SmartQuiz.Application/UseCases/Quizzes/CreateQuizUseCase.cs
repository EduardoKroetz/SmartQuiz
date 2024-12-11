using SmartQuiz.Application.DTOs.Records;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class CreateQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto<IdResult>> Execute(EditorQuizDto createQuizDto, Guid userId)
    {
        if (!Enum.TryParse(createQuizDto.Difficulty, true, out EDifficulty difficulty))
            throw new InvalidOperationException("Dificuldade inválida. Dificuldades disponíveis: easy, medium, hard");

        var quiz = new Quiz
        {
            Title = createQuizDto.Title,
            Description = createQuizDto.Description,
            Difficulty = difficulty,
            Theme = createQuizDto.Theme,
            Expires = createQuizDto.Expires,
            ExpiresInSeconds = createQuizDto.ExpiresInSeconds,
            IsActive = false,
            UserId = userId
        };

        await _quizRepository.AddAsync(quiz);

        return new ResultDto<IdResult>(new IdResult(quiz.Id));
    }
}