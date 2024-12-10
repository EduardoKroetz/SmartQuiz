using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartQuiz.API.Extensions;
using SmartQuiz.Application.UseCases.Quizzes;
using SmartQuiz.Core.DTOs.Quizzes;

namespace SmartQuiz.API.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] EditorQuizDto dto, [FromServices] CreateQuizUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(dto, userId);
        return Ok(result);
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
    public async Task<IActionResult> SearchQuizAsync([FromServices] SearchQuizUseCase useCase, [FromQuery] int pageSize = 25, [FromQuery] int pageNumber = 1, [FromQuery] string? reference = null)
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
    public async Task<IActionResult> SearchQuizByReviewsAsync([FromServices] SearchQuizByReviewsUseCase useCase, [FromQuery] int pageSize = 25, [FromQuery] int pageNumber = 1, [FromQuery] string? reference = null)
    {
        var result = await useCase.Execute(reference, pageSize, pageNumber);
        return Ok(result);
    }

    /// <summary>
    /// Ativa/desativa o Quiz
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPost("toggle/{quizId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleQuizAsync([FromRoute] Guid quizId, [FromServices] ToggleQuizUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(quizId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Atualiza as informações de um Quiz
    /// </summary>
    /// <param name="editorQuizDto"></param>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    /// <response code="200">Atualizado com sucesso</response>
    /// <response code="403">Quem está atualizando não é quem criou o Quiz</response>
    [HttpPut("{quizId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateQuizAsync([FromBody] EditorQuizDto editorQuizDto, [FromRoute] Guid quizId, [FromServices] UpdateQuizUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(quizId, editorQuizDto, userId);
        return Ok(result);
    }

    /// <summary>
    /// Deleta um Quiz pelo Id
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    /// <response code="400">Caso hajam partidas relacionadas</response>
    [HttpDelete("{quizId:guid}"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteQuizAsync([FromRoute] Guid quizId, [FromServices] DeleteQuizUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(quizId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Buscar todas as questões do Quiz
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet("{quizId:guid}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestionByQuiz([FromRoute] Guid quizId, [FromServices] GetQuestionsByQuizUseCase useCase)
    {
        var result = await useCase.Execute(quizId);
        return Ok(result);
    }
    
    /// <summary>
    /// Gerar um quiz automaticamente
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPost("generate"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateQuizAsync([FromBody] GenerateQuizDto generateQuizDto, [FromServices] GenerateQuizUseCase useCase)
    {
        var userId = User.GetUserId();
        var result = await useCase.Execute(generateQuizDto, userId);
        return Ok(result);
    }
}
