using SmartQuiz.Application.DTOs.Records;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class CreateQuizUseCase
{
    private readonly IQuizService _quizService;

    public CreateQuizUseCase(IQuizService quizService)
    {
        _quizService = quizService;
    }

    public async Task<ResultDto<IdResult>> Execute(EditorQuizDto createQuizDto, Guid userId)
    {
        var quiz = _quizService.CreateQuiz(createQuizDto, userId);

        await _quizService.AddAsync(quiz);

        return new ResultDto<IdResult>(new IdResult(quiz.Id));
    }
}