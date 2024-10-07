
using System.ComponentModel.DataAnnotations;

namespace QuizDev.Core.DTOs.Quizzes;

public class CreateQuizDto
{
    [Required(ErrorMessage = "Informe o título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Informe a descrição")]
    public string Description { get; set; }

    public bool Expires { get; set; }
    public int ExpiresInSeconds { get; set; }
}
