using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.OAuth;

public class ProcessGoogleCallbackUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IOAuthService _oauthService;
    private readonly string _redirectUri;
    private readonly string _frontendUrl;
    
    public ProcessGoogleCallbackUseCase(IUserRepository userRepository, IAuthService authService, IOAuthService oAuthService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _authService = authService;
        _oauthService = oAuthService;
        _redirectUri = configuration["GoogleOAuth:RedirectUri"] ?? throw new NullReferenceException("RedirectUri is invalid");
        _frontendUrl = configuration["FrontendUrl"] ?? throw new NullReferenceException("FrontendUrl is invalid");
    }
    
    public async Task<string> Execute(string code)
    {
        // Inicializar o fluxo de autorização
        var flow = _oauthService.InitializeGoogleFlow();
        
        // Trocar o código pelo token
        var token = await flow.ExchangeCodeForTokenAsync("user-id", code, _redirectUri, CancellationToken.None);
        
        //Criar um cliente de serviço com o token
        var userCredential = new UserCredential(flow, "user-id", token);
        
        var oauth2Service = new Oauth2Service(new BaseClientService.Initializer
        {
            HttpClientInitializer = userCredential,
            ApplicationName = "SmartQuiz",
        });
        
        //Pegar as informações do usuário com o serviço
        var userInfo = await oauth2Service.Userinfo.Get().ExecuteAsync();
        
        // Verificar / Criar usuário 
        var user = await _userRepository.GetByEmailAsync(userInfo.Email);
        if (user is null)
        {   
            user = new User
            {
                Username = userInfo.Name,
                Email = userInfo.Email,
                PasswordHash = "",
                IsOAuthUser = true
            };
            
            await _userRepository.AddAsync(user);
        }
        else if (!user.IsOAuthUser)
        {
            user.IsOAuthUser = true;
            await _userRepository.UpdateAsync(user);
        }

        var jwtToken = _authService.GenerateJwtToken(user);
        return $"{_frontendUrl}/callback?token={jwtToken}";
    }
}