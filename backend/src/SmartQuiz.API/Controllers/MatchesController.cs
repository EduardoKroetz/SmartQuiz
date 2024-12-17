using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartQuiz.API.Extensions;
using SmartQuiz.Application.DTOs.Matches;
using SmartQuiz.Application.UseCases.Matches;
using SmartQuiz.Application.UseCases.Responses;

namespace SmartQuiz.API.Controllers;

[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    /// <summary>
    /// Cria uma nova partida para jogar um Quiz
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns>Retorna a próxima questão a ser respondida e o Id da partida</returns>
    /// <response code="200" >Retorna o Id da partida e a primeira questão (objeto)</response>
    [HttpPost("play/quiz/{quizId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMatchAsync([FromRoute] Guid quizId, [FromServices] CreateMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(quizId, userId);
        return Ok(result); //alterar para o endpoint posteriormente
    }

    /// <summary>
    /// Enviar resposta para uma questão de uma partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="optionId"></param>
    /// <param name="useCase"></param>
    /// <response code="200">Retorna o Id da resposta criada e o Id da partida</response>
    [HttpPost("{matchId:guid}/submit/{optionId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitResponseAsync([FromRoute] Guid matchId, [FromRoute] Guid optionId, [FromServices] CreateResponseUseCase useCase)
    {
        var userId = User.GetUserId();
        var response = await useCase.Execute(userId, matchId, optionId);
        return Ok(response);
    }

    /// <summary>
    /// Buscar a próxima questão a ser respondida da partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet("{matchId:guid}/next-question"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetNextQuestion([FromRoute] Guid matchId, [FromServices] GetNextQuestionUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(matchId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Buscar detalhes da partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    /// <response code="200">Retorna os detalhes da partida (Não inclui relações com outras entidades)</response>
    /// <response code="404">Não encontrou a partida</response>
    /// <response code="403">Acessou partida de outro usuário</response>

    [HttpGet("{matchId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMatchDetails([FromRoute] Guid matchId, [FromServices] GetMatchUseCase useCase)
    {
        var result = await useCase.Execute(matchId);
        return Ok(result);
    }

    /// <summary>
    /// Finalizar partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    /// <response code="403">Tentou finalizar partida de outro usuário</response>
    [HttpPost("{matchId:guid}/end"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> EndMatchAsync([FromRoute] Guid matchId, [FromServices] FinalizeMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(matchId, userId);
        return Ok(result);
    }


    /// <summary>
    /// Busca todas as respostas de uma partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet("{matchId:guid}/responses"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetResponsesAsync([FromRoute] Guid matchId, [FromServices] GetResponsesByMatchUseCase useCase)
    {
        var result = await useCase.Execute(matchId);
        return Ok(result);
    }

    /// <summary>
    /// Deletar partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpDelete("{matchId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMatchAsync([FromRoute] Guid matchId, [FromServices] DeleteMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(matchId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Buscar partidas
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <param name="reference">Parâmetro de pesquisa para buscar por título ou descrição de um Quiz relacionado</param>
    /// <param name="status">Status da partida. Valores: created, finished, failed </param>
    /// <param name="reviewed">Se foi revisado</param>
    /// <param name="orderBy">Ordernar. Valores: score, created_at</param>
    /// <returns></returns>
    [HttpGet, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMatchesAsync(
        [FromServices] GetMatchesUseCase useCase,
        [FromQuery] GetMatchesDto dto)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(userId, dto);
        return Ok(result);
    }
    
    /// <summary>
    /// Falhar partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPatch("{matchId:guid}/fail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FailMatchAsync([FromRoute] Guid matchId, [FromServices] FailMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        await useCase.Execute(userId, matchId);
        return NoContent();
    }
}
