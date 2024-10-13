
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Responses;

public class CreateResponseUseCase
{
    private readonly IAnswerOptionRepository _answerOptionRepository;
    private readonly IResponseRepository _matchResponseRepository;
    private readonly IMatchRepository _matchRepository;

    public CreateResponseUseCase(IAnswerOptionRepository answerOptionRepository, IResponseRepository matchResponseRepository, IMatchRepository matchRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _matchResponseRepository = matchResponseRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid userId ,Guid matchId, Guid answerOptionId)
    {
        //Buscar opção
        var answerOption = await _answerOptionRepository.GetById(answerOptionId);
        if (answerOption == null)
        {
            throw new ArgumentException("Opção não encontrada");
        }

        //Buscar partida
        var match = await _matchRepository.GetAsync(matchId);
        if (match == null)
        {
            throw new ArgumentException("Partida não encontrada");
        }

        if (match.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão acessar esse recurso");
        }

        if (match.Status == Core.Enums.EMatchStatus.Finished)
        {
            throw new InvalidOperationException("Essa partida já foi finalizada");
        }

        if (match.Status == Core.Enums.EMatchStatus.Failed)
        {
            throw new InvalidOperationException("Partida expirada");
        }

        //Verificar se o tempo de expiração já passou
        if (match.AlreadyExpiration())
        {
            //Finalizar a partida caso já tenha expirado
            match.Status = Core.Enums.EMatchStatus.Failed;
            await _matchRepository.UpdateAsync(match);

            throw new InvalidOperationException("Partida expirada");
        }

        //Criar resposta
        var matchResponse = match.CreateResponse(answerOption);

        await _matchResponseRepository.CreateAsync(matchResponse);

        //Finaliza a partida caso essa seja a última questão
        if (answerOption.Question.Order + 1 == match.Quiz.Questions.Count)
        {
            match.Status = Core.Enums.EMatchStatus.Finished;
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
