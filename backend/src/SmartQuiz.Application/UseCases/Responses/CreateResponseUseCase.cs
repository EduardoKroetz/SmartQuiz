using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Responses;

public class CreateResponseUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IResponseRepository _responseRepository;

    public CreateResponseUseCase(IAnswerOptionRepository answerOptionRepository,
        IResponseRepository matchResponseRepository, IMatchRepository matchRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _responseRepository = matchResponseRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid userId, Guid matchId, Guid answerOptionId)
    {
        //Buscar opção
        var answerOption = await _answerOptionRepository.GetByIdAsync(answerOptionId);
        if (answerOption == null) throw new ArgumentException("Opção não encontrada");

        //Buscar partida
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null) throw new ArgumentException("Partida não encontrada");

        if (match.UserId != userId)
            throw new UnauthorizedAccessException("Você não tem permissão acessar esse recurso");

        if (match.Status == EMatchStatus.Finished)
            throw new InvalidOperationException("Essa partida já foi finalizada");

        if (match.Status == EMatchStatus.Failed) throw new InvalidOperationException("Partida expirada");

        //Verificar se o tempo de expiração já passou
        if (match.AlreadyExpiration() && match.Quiz.Expires)
        {
            //Finalizar a partida caso já tenha expirado
            match.Status = EMatchStatus.Failed;
            await _matchRepository.UpdateAsync(match);

            throw new InvalidOperationException("Partida expirada");
        }

        //Criar resposta
        var matchResponse = match.CreateResponse(answerOption);

        await _responseRepository.AddAsync(matchResponse);

        //Finaliza a partida caso essa seja a última questão
        if (answerOption.Question.Order + 1 == match.Quiz.Questions.Count)
        {
            match.Status = EMatchStatus.Finished;
            await _matchRepository.UpdateAsync(match);
        }

        //Após criação da resposta, adicionar pontuação caso a resposta esteja correta
        if (answerOption.IsCorrectOption)
        {
            match.AddScore();
            await _matchRepository.UpdateAsync(match);
        }

        return new ResultDto(new { ResponseId = matchResponse.Id, MatchId = matchId });
    }
}