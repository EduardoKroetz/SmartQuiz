using Microsoft.AspNetCore.Mvc;
using QuizDev.API.Extensions;
using QuizDev.Application.UseCases.Users;
using QuizDev.Core.DTOs.Accounts;
using QuizDev.Core.DTOs.Responses;

namespace QuizDev.API.Controllers;

[Route("api/[controller]/[action]")]
public class AccountsController : ControllerBase
{
  
    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto createUserDto, [FromServices] CreateUserUseCase useCase)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultDto(ModelState.GetErrors()));
        }

        var result = await useCase.Execute(createUserDto);
        return Ok(result);
    }
}
