using AutoMapper;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GetQuestionsByQuizUseCase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public GetQuestionsByQuizUseCase(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid quizId)
    {
        var questions = await _questionRepository.GetQuestionsByQuizId(quizId);

        var questionsDto = _mapper.Map<IEnumerable<GetQuestionWithCorrectOptionDto>>(questions);     
        
        return new ResultDto(questionsDto);
    }
}