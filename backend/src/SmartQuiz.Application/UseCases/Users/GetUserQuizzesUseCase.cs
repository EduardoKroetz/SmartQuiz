using AutoMapper;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Results;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Users;

public class GetUserQuizzesUseCase
{
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;

    public GetUserQuizzesUseCase(IQuizRepository quizRepository, IMapper mapper)
    {
        _quizRepository = quizRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResultDto> Execute(Guid userId, int pageSize, int pageNumber)
    {
        var skip = pageSize * (pageNumber - 1);
        var quizzes = await _quizRepository.GetUserQuizzesAsync(userId, skip, pageSize);
        var quizzesDto = _mapper.Map<IEnumerable<GetQuizDto>>(quizzes);
        return new PaginatedResultDto(pageSize, pageNumber, quizzes.Count, pageNumber + 1, quizzesDto);
    }
}