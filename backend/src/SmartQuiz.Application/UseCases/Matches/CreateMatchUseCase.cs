using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Matches;

public class CreateMatchUseCase
{
    private readonly IQuizService _quizService;
    private readonly IMatchService _matchService;

    public CreateMatchUseCase(IQuizService quizService, IMatchService matchService)
    {
        _quizService = quizService;
        _matchService = matchService;
    }

    public async Task<ResultDto> Execute(Guid quizId, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId);
        if (quiz == null) 
            throw new NotFoundException("Quiz não encontrado");

        _quizService.VerifyQuizActivation(quiz);

        var match = _matchService.CreateMatch(userId, quizId);

        await _matchService.AddAsync(match);

        return new ResultDto(new { MatchId = match.Id, match.ExpiresIn });
    }
}