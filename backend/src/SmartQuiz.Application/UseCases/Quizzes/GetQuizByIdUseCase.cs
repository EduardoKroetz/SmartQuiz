using AutoMapper;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GetQuizByIdUseCase
{
    private readonly IQuizService _quizService;
    private readonly IMapper _mapper;

    public GetQuizByIdUseCase(IQuizService quizService, IMapper mapper)
    {
        _quizService = quizService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
        if (quiz == null) 
            throw new ArgumentException("Quiz não encontrado");

        var quizDto = _mapper.Map<GetQuizDto>(quiz);

        return new ResultDto(quizDto);
    }
}