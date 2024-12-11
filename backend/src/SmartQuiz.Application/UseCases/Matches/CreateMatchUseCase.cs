using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Matches;

public class CreateMatchUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IQuizRepository _quizRepository;

    public CreateMatchUseCase(IMatchRepository matchRepository, IQuizRepository quizRepository)
    {
        _matchRepository = matchRepository;
        _quizRepository = quizRepository;
    }

    public async Task<ResultDto> Execute(Guid quizId, Guid userId)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null) 
            throw new NotFoundException("Quiz não encontrado");

        if (quiz.IsActive == false)
            throw new InvalidOperationException("Não é possível criar uma partida pois o Quiz está inativo");

        var match = new Match
        {
            QuizId = quizId,
            Quiz = quiz,
            Reviewed = false,
            Score = 0,
            UserId = userId,
            Status = EMatchStatus.Created
        };

        await _matchRepository.AddAsync(match);

        return new ResultDto(new { MatchId = match.Id, match.ExpiresIn });
    }
}