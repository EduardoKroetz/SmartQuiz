using AutoMapper;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Questions;

public class GetQuestionDetailsUseCase
{
    private readonly IQuestionService _questionService;
    private readonly IMapper _mapper;

    public GetQuestionDetailsUseCase(IQuestionService questionService, IMapper mapper)
    {
        _questionService = questionService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid questionId)
    {
        var question = await _questionService.GetByIdAsync(questionId);
        if (question == null) 
            throw new NotFoundException("Questão não encontrada");

        var questionDto = _mapper.Map<GetQuestionDto>(question);
        
        return new ResultDto(questionDto);
    }
}