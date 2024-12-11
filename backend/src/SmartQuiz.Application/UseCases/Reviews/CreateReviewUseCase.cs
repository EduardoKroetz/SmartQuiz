using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class CreateReviewUseCase
{
    private readonly IMatchRepository _matchRepository;
    private readonly IReviewRepository _reviewRepository;

    public CreateReviewUseCase(IReviewRepository reviewRepository, IMatchRepository matchRepository)
    {
        _reviewRepository = reviewRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(CreateReviewDto dto, Guid userId)
    {
        var match = await _matchRepository.GetByIdAsync(dto.MatchId);
        if (match == null) throw new NotFoundException("Partida não encontrada");

        if (match.Status != EMatchStatus.Finished)
            throw new InvalidOperationException("Não é possível criar avaliação para uma partida não concluída");

        if (match.Reviewed) throw new InvalidOperationException("Já foi criada uma avaliação para a partida");

        if (match.UserId != userId)
            throw new UnauthorizedAccessException(
                "Você não tem permissão para criar avaliação de partidas de outros usuários");

        var review = new Review
        {
            Description = dto.Description,
            Rating = dto.Rating,
            MatchId = dto.MatchId,
            QuizId = match.QuizId,
            UserId = userId
        };

        await _reviewRepository.AddAsync(review);

        match.ReviewId = review.Id;
        match.Reviewed = true;

        await _matchRepository.UpdateAsync(match);

        return new ResultDto(new { review.Id });
    }
}