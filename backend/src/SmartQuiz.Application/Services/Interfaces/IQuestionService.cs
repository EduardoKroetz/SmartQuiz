using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Core.Entities;

namespace SmartQuiz.Application.Services.Interfaces;

public interface IQuestionService
{
    Task<Question?> GetByIdAsync(Guid questionId);
    Task AddAsync(Question question);
    Task DeleteAsync(Question question);
    Task UpdateAsync(Question question);
    Task<IEnumerable<Question>> GetQuestionsByQuizId(Guid quizId);
    
    void RemoveQuestionFromQuiz(Quiz quiz, Question question);
    void AdjustQuestionsOrder(List<Question> questions, Question question);
    Question CreateQuestion(CreateQuestionDto dto);
    Question UpdateQuestion(Question question, string text);
    void UpdateCorrectOption(Question question, Guid newCorrectOptionId);
}