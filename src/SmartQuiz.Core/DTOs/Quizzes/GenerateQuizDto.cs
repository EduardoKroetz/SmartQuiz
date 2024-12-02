using System.ComponentModel.DataAnnotations;

namespace SmartQuiz.Core.DTOs.Quizzes;

public class GenerateQuizDto
{
    [Required(ErrorMessage = "Informe o tema do Quiz")]
    public string Theme { get; set; }
    
    [Required(ErrorMessage = "Informe a dificuldade")]
    public string Difficulty { get; set; }
    
    [Required(ErrorMessage = "Informe o número de questões")]
    public int NumberOfQuestions { get; set; }

    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
}