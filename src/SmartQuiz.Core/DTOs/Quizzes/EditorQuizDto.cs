using System.ComponentModel.DataAnnotations;

namespace SmartQuiz.Core.DTOs.Quizzes;

public class EditorQuizDto
{
    [Required(ErrorMessage = "Informe o título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Informe a descrição")]
    public string Description { get; set; }

    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }

    [Required(ErrorMessage = "Informe a dificuldade")]
    public string Difficulty { get; set; }
    
    [Required(ErrorMessage = "Informe o tema do Quiz")]
    public string Theme { get; set; }
}
