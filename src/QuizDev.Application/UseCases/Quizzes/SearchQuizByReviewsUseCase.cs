
using QuizDev.Core.DTOs.Quizzes;
using QuizDev.Core.DTOs.Results;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

public class SearchQuizByReviewsUseCase
{
    private readonly IQuizRepository _quizRepository;

    public SearchQuizByReviewsUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<PaginatedResultDto> Execute(string? reference, int pageSize, int pageNumber)
    {
        var keyWords = reference?.Split(" ");
        var skip = pageSize * ( pageNumber - 1 );

        var quizzes = await _quizRepository.SearchQuizByReviews(keyWords, skip, pageSize);

        var quizzesDto = quizzes.Select(x => new GetQuizDto(x.Id, x.Title, x.Description, x.Expires, x.ExpiresInSeconds, x.IsActive, x.UserId)).ToList();  

        return new PaginatedResultDto(pageSize, pageNumber, quizzes.Count, pageNumber + 1, quizzesDto);
    }
}
