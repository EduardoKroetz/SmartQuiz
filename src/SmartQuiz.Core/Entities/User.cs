namespace SmartQuiz.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public List<Match> Matchs { get; set; }
    public List<Quiz> Quizes { get; set; }
    public List<Review> Reviews { get; set; }
    public bool EmailIsVerified { get; set; }
}
