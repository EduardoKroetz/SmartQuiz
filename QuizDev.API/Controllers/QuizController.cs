using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Quizzes;
using QuizDev.Core.DTOs.Quizzes;

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
        return Created(nameof(result), result);
    }

}
