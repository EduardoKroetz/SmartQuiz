using Microsoft.Extensions.Configuration;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.OAuth;

public class LoginWithGoogleUseCase
{
    private readonly IOAuthService _oAuthService;
    private readonly string _redirectUri;

    public LoginWithGoogleUseCase(IOAuthService oAuthService, IConfiguration configuration)
    {
        _oAuthService = oAuthService;
        _redirectUri = configuration["GoogleOAuth:RedirectUri"] ?? throw new NullReferenceException("RedirectUri is invalid");
    }

    public string Execute()
    {
        var flow = _oAuthService.InitializeGoogleFlow();
        return flow.CreateAuthorizationCodeRequest(_redirectUri).Build().AbsoluteUri;
    }
}