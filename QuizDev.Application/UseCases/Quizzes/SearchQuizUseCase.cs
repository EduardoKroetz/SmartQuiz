
using QuizDev.Application.DTOs.Results;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

public class SearchQuizUseCase
{
    private readonly IQuizRepository _quizRepository;

    public SearchQuizUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<PaginatedResultDto> Execute(string reference, int pageSize, int pageNumber)
    {
        var keyWords = reference.Split(" ");
        var skip = pageSize * (pageNumber - 1 );

        var quizzes = await _quizRepository.SearchQuiz(keyWords, skip, pageSize);

        return new PaginatedResultDto(pageSize, pageNumber, quizzes.Count, pageNumber + 1, quizzes);  
    }
}
