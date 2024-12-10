
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

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
