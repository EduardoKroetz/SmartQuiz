using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IMatchService
{
    Task<Match?> GetByIdAsync(Guid matchId);
    Task AddAsync(Match match);
    Task DeleteAsync(Match match);
    Task<IEnumerable<GetMatchDto>> GetMatchesAsync(GetMatchesDto dto, Guid userId);
    Task<Question> GetNextQuestion(Match match);

    Match CreateMatch(Guid userId, Guid quizId);
    void FailMatch(Match match);
    void FinalizeMatch(Match match);
    Task UpdateAsync(Match match);
    void EnsureNotCompleted(Match match);
    void AddMatchScore(Match match);
    bool AlreadyMatchExpired(Match match);
    void AddMatchReview(Match match, Review review);
    void RemoveMatchReview(Match match);
}