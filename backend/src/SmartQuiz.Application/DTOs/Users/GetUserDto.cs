namespace SmartQuiz.Application.DTOs.Users;

public class GetUserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool EmailIsVerified { get; set; }
    public int TotalScore { get; set; }
    public int MaxScore { get; set; }
    public int MatchesPlayed { get; set; }
    public int CreatedQuizzes { get; set; }
}