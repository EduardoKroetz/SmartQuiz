using System.ComponentModel.DataAnnotations;

namespace SmartQuiz.Application.DTOs.Quizzes;

public class GenerateQuizDto
{
    [Required(ErrorMessage = "Informe o tema do Quiz")]
    public string Theme { get; set; }

    [Required(ErrorMessage = "Informe a dificuldade")]
    public string Difficulty { get; set; }

    [Required(ErrorMessage = "Informe o número de questões")]
    [Range(1, 20, ErrorMessage = "O número de questões deve ser entre 1 e 20")]
    public int NumberOfQuestions { get; set; }

    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
}