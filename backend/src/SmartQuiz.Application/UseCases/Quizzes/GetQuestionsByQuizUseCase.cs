
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GetQuestionsByQuizUseCase
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionsByQuizUseCase(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid quizId)
    {
        var questions = await _questionRepository.GetQuestionsByQuizId(quizId);
        return new ResultDto(questions);
    }
}
