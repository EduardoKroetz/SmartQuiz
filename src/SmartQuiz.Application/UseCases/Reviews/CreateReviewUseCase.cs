
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.DTOs.Reviews;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class CreateReviewUseCase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMatchRepository _matchRepository;

    public CreateReviewUseCase(IReviewRepository reviewRepository, IMatchRepository matchRepository)
    {
        _reviewRepository = reviewRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(CreateReviewDto dto, Guid userId)
    {
        var match = await _matchRepository.GetAsync(dto.MatchId);
        if (match == null)
        {
            throw new NotFoundException("Partida não encontrada");
        }

        if (match.Status != Core.Enums.EMatchStatus.Finished)
        {
            throw new InvalidOperationException("Não é possível criar avaliação para uma partida não concluída");
        }

        if (match.Reviewed)
        {
            throw new InvalidOperationException("Já foi criada uma avaliação para a partida");
        }

        if (match.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para criar avaliação de partidas de outros usuários");
        }

        var review = new Review
        {
            Id = Guid.NewGuid(),
            Description = dto.Description,
            Rating = dto.Rating,
            MatchId = dto.MatchId,
            QuizId = match.QuizId,
            UserId = userId,
        };

        await _reviewRepository.CreateAsync(review);

        match.ReviewId = review.Id;
        match.Reviewed = true;

        await _matchRepository.UpdateAsync(match);

        return new ResultDto(new { review.Id });
    }
}
