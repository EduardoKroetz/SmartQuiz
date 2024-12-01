using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GetQuizByIdUseCase
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizByIdUseCase(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(Guid id)
    {
        var quiz = await _quizRepository.GetAsync(id, true);
        if (quiz == null)
        {
            throw new ArgumentException("Quiz não encontrado");
        }

        var quizDto = new GetQuizDto(quiz.Id, quiz.Title, quiz.Description, quiz.Expires, quiz.ExpiresInSeconds, quiz.IsActive, quiz.UserId);

        return new ResultDto(quizDto);
    }
}
