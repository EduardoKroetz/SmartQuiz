namespace SmartQuiz.Application.Services.Interfaces;

public interface IGeminiService
{
    Task<string> RequestGeminiAsync(string prompt);
}