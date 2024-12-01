
namespace SmartQuiz.Core.DTOs.Users;

public class GetUserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool EmailIsVerified { get; set; }
}
