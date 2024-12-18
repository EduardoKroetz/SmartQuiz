using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IQuizService
{
    Task<Quiz?> GetByIdAsync(Guid quizId);
    Task AddAsync(Quiz quiz);
    void VerifyQuizActivation(Quiz quiz);
    Task DeleteAsync(Quiz quiz);
    Task UpdateAsync(Quiz quiz);
    Task<bool> HasMatchesRelated(Guid quizId);
    Task<IEnumerable<Quiz>> SearchQuizAsync(SearchQuizDto searchQuizDto);
    
    Quiz CreateQuiz(EditorQuizDto dto, Guid userId);
    void ToggleQuiz(Quiz quiz);
    Quiz UpdateQuiz(Quiz quiz, EditorQuizDto editorQuizDto);
}