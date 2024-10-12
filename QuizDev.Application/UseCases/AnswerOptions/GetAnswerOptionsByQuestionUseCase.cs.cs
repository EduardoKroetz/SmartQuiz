
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.AnswerOptions;

public class GetAnswerOptionsByQuestionUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;

    public GetAnswerOptionsByQuestionUseCase(IAnswerOptionRepository answerOptionRepository)
    {
        _answerOptionRepository = answerOptionRepository;
    }

    public async Task<ResultDto> Execute(Guid questionId)
    {
        var answerOptions = await _answerOptionRepository.GetByQuestionId(questionId);
        return new ResultDto(answerOptions);
    }
}
