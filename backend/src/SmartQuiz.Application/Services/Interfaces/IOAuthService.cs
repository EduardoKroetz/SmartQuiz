using Google.Apis.Auth.OAuth2.Flows;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IOAuthService
{ 
    GoogleAuthorizationCodeFlow InitializeGoogleFlow();
}