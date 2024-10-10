using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Matches;
using QuizDev.Application.UseCases.Responses;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    /// <summary>
    /// Cria uma nova partida para jogar um Quiz
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns>Retorna a próxima questão a ser respondida e o Id da partida</returns>
    /// <response code="201" >Retorna o Id da partida e a primeira questão (objeto)</response>
    [HttpPost("play/quiz/{quizId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMatchAsync([FromRoute] Guid quizId, [FromServices] CreateMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(quizId, userId);
        return Created(nameof(result), result); //alterar para o endpoint posteriormente
    }

    /// <summary>
    /// Cria resposta para uma questão de uma partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="optionId"></param>
    /// <param name="useCase"></param>
    /// <response code="201">Retorna o Id da resposta criada e o Id da partida</response>
    [HttpPost("{matchId:guid}/submit/{optionId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitResponseAsync([FromRoute] Guid matchId, [FromRoute] Guid optionId, [FromServices] CreateResponseUseCase useCase)
    {
        var userId = User.GetUserId();
        var response = await useCase.Execute(userId ,matchId, optionId);
        return Created(nameof(response) ,response);
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

    [HttpGet("{matchId:guid}"),  Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMatchDetails([FromRoute] Guid matchId, [FromServices] GetMatchUseCase useCase) 
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(matchId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Finalizar partida
    /// </summary>
    /// <param name="matchId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    /// <response code="403">Tentou finalizar partida de outro usuário</response>
    [HttpPost("{matchId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> FinalizeMatchAsync([FromRoute] Guid matchId, [FromServices] FinalizeMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(matchId, userId);
        return Ok(result);
    }

}
