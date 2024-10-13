using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Reviews;
using QuizDev.Core.DTOs.Reviews;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    
    /// <summary>
    /// Criar avaliação para uma partida
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateReviewDto dto, [FromServices] CreateReviewUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(dto, userId);
        return Ok(result);
    }

}
