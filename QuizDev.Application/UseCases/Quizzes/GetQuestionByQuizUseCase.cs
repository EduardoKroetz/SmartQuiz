
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

public class GetQuestionByQuizUseCase
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionByQuizUseCase(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid quizId)
    {
        var questions = await _questionRepository.GetQuestionByQuizId(quizId);
        return new ResultDto(questions);
    }
}
