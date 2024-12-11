using AutoMapper;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GetQuizByIdUseCase
{
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;

    public GetQuizByIdUseCase(IQuizRepository quizRepository, IMapper mapper)
    {
        _quizRepository = quizRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> Execute(Guid id)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        if (quiz == null) 
            throw new ArgumentException("Quiz não encontrado");

        var quizDto = _mapper.Map<GetQuizDto>(quiz);

        return new ResultDto(quizDto);
    }
}