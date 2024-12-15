namespace SmartQuiz.Application.Validators.Interfaces;

public interface IUserAuthorizationValidator
{
    void ValidateAuthorization(Guid userId, Guid autheticatedUserId);
}