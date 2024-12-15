using AutoMapper;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.AnswerOptions;

public class GetAnswerOptionsByQuestionUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;
    private readonly IMapper _mapper;

    public GetAnswerOptionsByQuestionUseCase(IAnswerOptionRepository answerOptionRepository, IMapper mapper)
    {
        _answerOptionRepository = answerOptionRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid questionId)
    {
        var answerOptions = await _answerOptionRepository.GetByQuestionId(questionId);
        var answerOptionDto = _mapper.Map<GetAnswerOptionDto>(answerOptions);
        return new ResultDto(answerOptionDto);
    }
}