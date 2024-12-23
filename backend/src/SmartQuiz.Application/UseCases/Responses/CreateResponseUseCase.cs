using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Responses;

public class CreateResponseUseCase
{
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IMatchService _matchService;
    private readonly IResponseService _responseService;
    private readonly IAuthService _authService;

    public CreateResponseUseCase(IAnswerOptionService answerOptionService,
        IResponseService matchResponseService, IMatchService matchService, IAuthService authService)
    {
        _answerOptionService = answerOptionService;
        _responseService = matchResponseService;
        _matchService = matchService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(Guid userId, Guid matchId, Guid answerOptionId)
    {
        //Buscar opção
        var answerOption = await _answerOptionService.GetByIdAsync(answerOptionId);
        if (answerOption == null) 
            throw new ArgumentException("Opção não encontrada");

        //Buscar partida
        var match = await _matchService.GetByIdAsync(matchId);
        if (match == null) 
            throw new ArgumentException("Partida não encontrada");

        _authService.ValidateSameUser(match.UserId, userId);

        _matchService.EnsureNotCompleted(match);

        if (_matchService.AlreadyMatchExpired(match))
        {
            _matchService.FailMatch(match);
            await _matchService.UpdateAsync(match);

            throw new InvalidOperationException("Partida expirada");
        }
        
        var matchResponse = _responseService.CreateResponse(match, answerOption);

        await _responseService.AddAsync(matchResponse);

        //Finaliza a partida caso essa seja a última questão
        if (answerOption.Question.Order + 1 == match.Quiz.Questions.Count)
        {
            _matchService.FinalizeMatch(match);
            await _matchService.UpdateAsync(match);
        }

        // Adicionar pontuação caso a resposta esteja correta
        if (answerOption.IsCorrectOption)
        {
            _matchService.AddMatchScore(match);
            await _matchService.UpdateAsync(match);
        }

        return new ResultDto(new { ResponseId = matchResponse.Id, MatchId = matchId });
    }
}