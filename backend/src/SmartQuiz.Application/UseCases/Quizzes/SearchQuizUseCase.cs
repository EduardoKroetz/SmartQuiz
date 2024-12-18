using AutoMapper;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Results;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class SearchQuizUseCase
{
    private readonly IQuizService _quizService;
    private readonly IMapper _mapper;

    public SearchQuizUseCase(IQuizService quizService, IMapper mapper)
    {
        _quizService = quizService;
        _mapper = mapper;
    }

    public async Task<PaginatedResultDto> Execute(SearchQuizDto searchQuizDto)
    {
        var quizzes = await _quizService.SearchQuizAsync(searchQuizDto);
        
        var quizzesDto = _mapper.Map<IEnumerable<GetQuizDto>>(quizzes);

        return new PaginatedResultDto(searchQuizDto.PageSize, searchQuizDto.PageNumber, quizzes.Count(), searchQuizDto.PageNumber + 1, quizzesDto);
    }
}