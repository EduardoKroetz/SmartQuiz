
using QuizDev.Application.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Responses;

public class CreateResponseUseCase
{
    private readonly IQuestionOptionRepository _questionOptionRepository;
    private readonly IResponseRepository _matchResponseRepository;
    private readonly IMatchRepository _matchRepository;

    public CreateResponseUseCase(IQuestionOptionRepository questionOptionRepository, IResponseRepository matchResponseRepository, IMatchRepository matchRepository)
    {
        _questionOptionRepository = questionOptionRepository;
        _matchResponseRepository = matchResponseRepository;
        _matchRepository = matchRepository;
    }

    public async Task<ResultDto> Execute(Guid userId ,Guid matchId, Guid questionOptionId)
    {
        //Buscar opção
        var questionOption = await _questionOptionRepository.GetById(questionOptionId);
        if (questionOption == null)
        {
            throw new ArgumentException("Opção não encontrada");
        }

        //Buscar partida
        var match = await _matchRepository.GetAsync(matchId, true);
        if (match == null)
        {
            throw new ArgumentException("Partida não encontrada");
        }

        if (match.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão acessar esse recurso");
        }

        //Criar resposta
        var matchResponse = match.CreateResponse(questionOption);

        await _matchResponseRepository.CreateAsync(matchResponse);

        //Após criação da resposta, adicionar pontuação caso a resposta esteja correta
        if (questionOption.IsCorrectOption)
        {
            match.AddScore();
            await _matchRepository.UpdateAsync(match);
        }

        return new ResultDto(new { ResponseId = matchResponse.Id, MatchId = matchId });
    }

}
