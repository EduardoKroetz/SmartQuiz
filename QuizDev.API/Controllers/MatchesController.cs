using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Matches;

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
    [HttpPost("play/quiz/{quizId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MatchAsync([FromRoute] Guid quizId, [FromServices] CreateMatchUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(quizId, userId);
        return Ok(result);
    }

}
