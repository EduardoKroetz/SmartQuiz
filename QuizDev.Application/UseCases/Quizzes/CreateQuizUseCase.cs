
using QuizDev.Core.DTOs.Quizzes;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

public class CreateQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(CreateQuizDto createQuizDto, Guid userId)
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
