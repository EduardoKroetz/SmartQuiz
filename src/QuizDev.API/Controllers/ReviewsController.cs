using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Reviews;
using QuizDev.Core.DTOs.Reviews;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class ReviewsController : ControllerBase
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

    /// <summary>
    /// Deletar avaliação de uma partida
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpDelete("{reviewId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid reviewId, [FromServices] DeleteReviewUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(reviewId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Atualizar avaliação
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="dto"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPut("{reviewId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid reviewId, [FromBody] UpdateReviewDto dto, [FromServices] UpdateReviewUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(dto, reviewId, userId);
        return Ok(result);
    }


    /// <summary>
    /// Buscar detalhes da avaliação
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailsAsync([FromRoute] Guid reviewId, [FromServices] GetReviewDetailsUseCase useCase)
    {
        var result = await useCase.Execute(reviewId);
        return Ok(result);
    }

}
