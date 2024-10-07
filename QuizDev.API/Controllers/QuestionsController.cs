using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizDev.Application.UseCases.Questions;
using QuizDev.Application.DTOs.Questions;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    /// <summary>
    /// Criar uma nova questão para um Quiz
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionDto dto, [FromServices] CreateQuestionUseCase useCase)
    {
        var result = await useCase.Execute(dto);
        return Ok(result);
    }

}
