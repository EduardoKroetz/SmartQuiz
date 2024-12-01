
using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class CreateQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(EditorQuizDto createQuizDto, Guid userId)
    {
        var quiz = new Quiz
        {
            Id = Guid.NewGuid(),
            Title = createQuizDto.Title,
            Description = createQuizDto.Description,
            Expires = createQuizDto.Expires,
            ExpiresInSeconds = createQuizDto.ExpiresInSeconds,
            IsActive = false,
            UserId = userId
        };

        await _quizRepository.CreateAsync(quiz);

        return new ResultDto(new { quiz.Id });
    }
}
