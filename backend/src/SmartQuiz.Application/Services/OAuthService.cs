using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.Services;

public class OAuthService : IOAuthService
{
    private readonly string _googleClientId;
    private readonly string _googleClientSecret;
    private readonly string[] _scopes;

    public OAuthService(IConfiguration configuration)
    {
        _googleClientId = configuration["GoogleOAuth:ClientId"] ?? throw new NullReferenceException("Google oauth client id is invalid");
        _googleClientSecret = configuration["GoogleOAuth:ClientSecret"] ?? throw new NullReferenceException("Google oauth client secret is invalid");
        _scopes = new[]
        {
            "https://www.googleapis.com/auth/userinfo.email",
            "https://www.googleapis.com/auth/userinfo.profile",
        };
    }
    
    public GoogleAuthorizationCodeFlow InitializeGoogleFlow()
    {
        return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleClientId,
                ClientSecret = _googleClientSecret
            },
            Scopes = _scopes,
        });
    }

}