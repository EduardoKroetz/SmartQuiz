using QuizDev.Application.DTOs.Quizzes;
using QuizDev.Application.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Quizzes;

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

        var quizDto = new GetQuizDto(quiz.Id, quiz.Title, quiz.Description, quiz.Expires, quiz.ExpiresInSeconds, quiz.IsActive, quiz.UserId, quiz.Questions);

        return new ResultDto(quizDto);
    }
}
