using Microsoft.AspNetCore.Mvc;
using QuizDev.Application.UseCases.Users;
using QuizDev.Core.DTOs.Users;

namespace QuizDev.API.Controllers;

[Route("api/[controller]")]
public class AccountsController : ControllerBase
{

    /// <summary>
    /// Registrar novo usuário
    /// </summary>
    /// <param name="createUserDto"></param>
    /// <returns>Token de autenticação e Id do usuário criado</returns>
    /// <response code="201">Retorna o token de autenticação e o Id do usuário criado</response>
    /// <response code="400">Se os parâmetros são inválidos</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto createUserDto, [FromServices] CreateUserUseCase useCase)
    {
        var result = await useCase.Execute(createUserDto);
        return Created(nameof(result), result); //Trocar para GetById quando tiver o método
    }

    /// <summary>
    /// Autenticar usuário com e-mail e senha
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Token de autenticação</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto dto, [FromServices] LoginUserUseCase useCase)
    {
        var result = await useCase.Execute(dto);
        return Ok(result);
    }
}
