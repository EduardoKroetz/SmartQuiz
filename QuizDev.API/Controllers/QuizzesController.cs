using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Quizzes;
using QuizDev.Application.DTOs.Quizzes;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    /// <summary>
    /// Criar um novo Quiz
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Id do Quiz criado</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateQuizDto dto, [FromServices] CreateQuizUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(dto, userId);
        return Created(nameof(GetQuizById), result);
    }

    /// <summary>
    /// Buscar dados do Quiz pelo ID
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet("{quizId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetQuizById([FromRoute] Guid quizId, [FromServices] GetQuizByIdUseCase useCase)
    {
        var result = await useCase.Execute(quizId);
        return Ok(result);
    }

    /// <summary>
    /// Pesquisar Quiz
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="useCase"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchQuizAsync([FromQuery] string reference, [FromServices] SearchQuizUseCase useCase, [FromQuery] int pageSize = 25, [FromQuery] int pageNumber = 1)
    {
        var result = await useCase.Execute(reference, pageSize, pageNumber);
        return Ok(result);
    }

    /// <summary>
    /// Pesquisar Quiz por Reviews e ordenar por maior avaliação
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="useCase"></param>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    [HttpGet("reviews/search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchQuizByReviewsAsync([FromQuery] string reference, [FromServices] SearchQuizByReviewsUseCase useCase, [FromQuery] int pageSize = 25, [FromQuery] int pageNumber = 1)
    {
        var result = await useCase.Execute(reference, pageSize, pageNumber);
        return Ok(result);
    }
}
