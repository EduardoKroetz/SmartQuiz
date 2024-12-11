using AutoMapper;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Questions;

public class GetQuestionDetailsUseCase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public GetQuestionDetailsUseCase(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid questionId)
    {
        var question = await _questionRepository.GetByIdAsync(questionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");

        var questionDto = _mapper.Map<GetQuestionDto>(question);
        
        return new ResultDto(questionDto);
    }
}