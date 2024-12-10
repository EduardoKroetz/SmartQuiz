using System.ComponentModel.DataAnnotations;

namespace SmartQuiz.Core.DTOs.Quizzes;

public class EditorQuizDto
{
    [Required(ErrorMessage = "Informe o título")]
    [MaxLength(50, ErrorMessage = "O título deve possuir no máximo 50 caracteres")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Informe a descrição")]
    [MinLength(45, ErrorMessage = "A descrição deve possuir no mínimo 55 caracteres")]
    [MaxLength(100, ErrorMessage = "A descrição deve possuir no máximo 100 caracteres")]
    public string Description { get; set; }

    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }

    [Required(ErrorMessage = "Informe a dificuldade")]
    public string Difficulty { get; set; }
    
    [Required(ErrorMessage = "Informe o tema do Quiz")]
    [MaxLength(50, ErrorMessage = "O tema deve possuir no máximo 50 caracteres")]
    public string Theme { get; set; }
}
