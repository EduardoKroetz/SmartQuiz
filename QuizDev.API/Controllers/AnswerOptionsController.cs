using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.AnswerOptions;
using QuizDev.Core.DTOs.AnswerOptions;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class AnswerOptionsController : ControllerBase
{
    /// <summary>
    /// Criar opção de resposta para uma Questão
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAnswerOptionAsync([FromBody] CreateAnswerOptionDto dto, [FromServices] CreateAnswerOptionUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(dto, userId);
        return Ok(result);
    }
        
}
