using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Enums;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;

    public QuizService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<Quiz?> GetByIdAsync(Guid quizId)
    {
        return await _quizRepository.GetByIdAsync(quizId);
    }

    public async Task AddAsync(Quiz quiz)
    {
        await _quizRepository.AddAsync(quiz);
    }

    public void VerifyQuizActivation(Quiz quiz)
    {
        if (quiz.IsActive == false)
            throw new InvalidOperationException("Não é possível criar uma partida pois o Quiz está inativo");
    }

    public async Task DeleteAsync(Quiz quiz)
    {
        await _quizRepository.DeleteAsync(quiz);
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        await _quizRepository.UpdateAsync(quiz);
    }

    public async Task<bool> HasMatchesRelated(Guid quizId)
    {
        return await _quizRepository.HasMatchesRelated(quizId);
    }

    public async Task<IEnumerable<Quiz>> SearchQuizAsync(SearchQuizDto searchQuizDto)
    {
        string[]? keyWords = null;
        if (searchQuizDto.Reference is not null)
        {
            keyWords = searchQuizDto.Reference.Split(" ");
            keyWords = keyWords.Where(x => x.Length > 1).ToArray();
        }
        
        var skip = searchQuizDto.PageSize * (searchQuizDto.PageNumber - 1);

        return await _quizRepository.SearchQuiz(keyWords, skip, searchQuizDto.PageSize, searchQuizDto.UserId);
    }

    public Quiz CreateQuiz(EditorQuizDto dto, Guid userId)
    {
        if (!Enum.TryParse(dto.Difficulty, true, out EDifficulty difficulty))
            throw new InvalidOperationException("Dificuldade inválida. Dificuldades disponíveis: easy, medium, hard");
        
        if (dto.ExpiresInSeconds < 10 & dto.Expires)
            throw new ArgumentException("A expiração do Quiz deve ser maior que 10 segundos");

        return new Quiz
        {
            Title = dto.Title,
            Description = dto.Description,
            Difficulty = difficulty,
            Theme = dto.Theme,
            Expires = dto.Expires,
            ExpiresInSeconds = dto.ExpiresInSeconds,
            IsActive = false,
            UserId = userId
        };
    }

    public void ToggleQuiz(Quiz quiz)
    {
        if (quiz.IsActive == false && quiz.Questions.Count == 0)
            throw new ArgumentException("Não é possível ativar o Quiz pois ele não possui nenhuma questão relacionada");

        quiz.IsActive = !quiz.IsActive;
    }

    public Quiz UpdateQuiz(Quiz quiz, EditorQuizDto editorQuizDto)
    {
        if (!Enum.TryParse(editorQuizDto.Difficulty, true, out EDifficulty difficulty))
            throw new InvalidOperationException("Dificuldade inválida. Dificuldades disponíveis: easy, medium, hard");

        quiz.Title = editorQuizDto.Title;
        quiz.Description = editorQuizDto.Description;
        quiz.Expires = editorQuizDto.Expires;
        quiz.ExpiresInSeconds = editorQuizDto.ExpiresInSeconds;
        quiz.Difficulty = difficulty;
        quiz.Theme = editorQuizDto.Theme;

        return quiz;
    }
}