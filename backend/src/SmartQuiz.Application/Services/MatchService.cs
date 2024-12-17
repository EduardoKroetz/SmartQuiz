using AutoMapper;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public MatchService(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        return await _matchRepository.GetByIdAsync(id);
    }
    
    public async Task AddAsync(Match match)
    {
        await _matchRepository.AddAsync(match);
    }

    public Match CreateMatch(Guid userId, Guid quizId)
    {
        return new Match
        {
            QuizId = quizId,
            Reviewed = false,
            Score = 0,
            UserId = userId,
            Status = EMatchStatus.Created
        };
    }

    public async Task DeleteAsync(Match match)
    {
        await _matchRepository.DeleteAsync(match);
    }

    public void FailMatch(Match match)
    {
        match.Status = EMatchStatus.Failed;
    }

    public void FinalizeMatch(Match match)
    {
        EnsureNotCompleted(match);
        
        match.Status = EMatchStatus.Finished;
    }

    public async Task UpdateAsync(Match match)
    {
        await _matchRepository.UpdateAsync(match);
    }

    public async Task<IEnumerable<GetMatchDto>> GetMatchesAsync(GetMatchesDto dto, Guid userId)
    {
        var skip = dto.PageSize * (dto.PageNumber - 1);
        var matches = await _matchRepository.GetMatchesAsync(userId, skip, dto.PageSize, dto.Reference, dto.Status, dto.Reviewed, dto.OrderBy);
        
        return _mapper.Map<IEnumerable<GetMatchDto>>(matches);
    }

    public void EnsureNotCompleted(Match match)
    {
        if (match.Status == EMatchStatus.Finished)
            throw new InvalidOperationException("Essa partida já foi finalizada");

        if (match.Status == EMatchStatus.Failed)
            throw new InvalidOperationException("Não é possível finalizar essa partida");
    }

    public async Task<Question> GetNextQuestion(Match match)
    {
        var nextQuestion = await _matchRepository.GetNextQuestion(match);
        
        if (nextQuestion is null)
        {
            if (match.Responses.Count == 0)
            {
                nextQuestion = match.Quiz.Questions.OrderBy(x => x.Order).FirstOrDefault();

                if (nextQuestion == null)
                    throw new ArgumentException("Não foi possível buscar a primeira questão do Quiz");
            }
            else
            {
                throw new ArgumentException("Não foi possível buscar a próxima questão");
            }
        }
        
        return nextQuestion;
    }

    public void AddMatchScore(Match match)
    {
        match.Score++;
    }

    public bool AlreadyMatchExpired(Match match)
    {
        var expires = match.ExpiresIn > DateTime.UtcNow;
        return expires && match.Quiz.Expires;
    }

    public void AddMatchReview(Match match, Review review)
    {
        match.ReviewId = review.Id;
        match.Reviewed = true;
    }

    public void RemoveMatchReview(Match match)
    {
        match.ReviewId = null;
        match.Reviewed = false;
    }

    public static int GetMatchRemainingTime(Match match)
    {
        var totalSeconds = (int)(match.ExpiresIn - DateTime.UtcNow).TotalSeconds;
        return totalSeconds < 0 ? 0 : totalSeconds;
    }
    
}