using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories.Base;

namespace SmartQuiz.Core.Repositories;

public interface IMatchRepository : IRepository<Match>
{
    Task<Question?> GetNextQuestion(Match match);
    Task<List<Match>> GetMatchesAsync(Guid userId, int skip, int take, string? reference = null, string? status = null, bool? reviewed = null, string? orderBy = null);
    Task<List<Match>> GetUserMatchesAsync(Guid userId, int skip, int take);
}
