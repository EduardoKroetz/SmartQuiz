using System.Security.Claims;

namespace QuizDev.API.Extensions;

public static class ClaimsPrincipalExtension 
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdStr = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Não foi possível obter o id do usuário");
        return new Guid(userIdStr);
    }
}
