using SmartQuiz.Application.Validators.Interfaces;

namespace SmartQuiz.Application.Validators;

public class UserAuthorizationValidator : IUserAuthorizationValidator
{
    public void ValidateAuthorization(Guid userId, Guid autheticatedUserId)
    {
        if (userId != autheticatedUserId)
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");
    }
}