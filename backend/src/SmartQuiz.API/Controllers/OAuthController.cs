using Microsoft.AspNetCore.Mvc;
using SmartQuiz.Application.UseCases.OAuth;

namespace SmartQuiz.API.Controllers;

[Route("api/[controller]")]
public class OAuthController : ControllerBase
{
    [HttpGet("google/login")]
    public IActionResult LoginWithGoogleAsync([FromServices] LoginWithGoogleUseCase useCase)
    {
        var redirectUri = useCase.Execute();
        return Redirect(redirectUri);
    }
    
    [HttpGet("google/callback")]
    public async Task<IActionResult> ProcessGoogleCallbackAsync([FromQuery] string code, [FromServices] ProcessGoogleCallbackUseCase useCase)
    {
        var frontendCallback = await useCase.Execute(code);
        return Redirect(frontendCallback);
    }
}