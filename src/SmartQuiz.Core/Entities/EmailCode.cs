
namespace SmartQuiz.Core.Entities;

public class EmailCode
{
    public EmailCode(string email)
    {

        Code = new Random().Next(100000, 999999).ToString();
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

    public string Code { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
