using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.DTOs.Reviews;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Reviews;

public class CreateReviewUseCase
{
    private readonly IReviewService _reviewService;
    private readonly IMatchService _matchService;
    private readonly IAuthService _authService;

    public CreateReviewUseCase(IReviewService reviewService, IMatchService matchService, IAuthService authService)
    {
        _reviewService = reviewService;
        _matchService = matchService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(CreateReviewDto dto, Guid userId)
    {
        var match = await _matchService.GetByIdAsync(dto.MatchId);
        if (match == null) 
            throw new NotFoundException("Partida não encontrada");

        if (match.Status != EMatchStatus.Finished)
            throw new InvalidOperationException("Não é possível criar avaliação para uma partida não concluída");

        if (match.Reviewed) 
            throw new InvalidOperationException("Já foi criada uma avaliação para a partida");

        _authService.ValidateSameUser(match.UserId, userId);

        var review = _reviewService.CreateReview(dto, match, userId);
        await _reviewService.AddAsync(review);

        _matchService.AddMatchReview(match, review);
        await _matchService.UpdateAsync(match);

        return new ResultDto(new { review.Id });
    }
}