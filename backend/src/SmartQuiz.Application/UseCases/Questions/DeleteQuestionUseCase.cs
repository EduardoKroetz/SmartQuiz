using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.DTOs.Questions;

public class DeleteQuestionUseCase
{
    private readonly IQuestionRepository _questionRepository;

    public DeleteQuestionUseCase(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ResultDto> Execute(Guid questionId, Guid userId)
    {
        var question = await _questionRepository.GetByIdAsync(questionId);
        if (question == null) throw new NotFoundException("Questão não encontrada");

        if (question.Quiz.UserId != userId)
            throw new UnauthorizedAccessException("Você não tem permissão para acessar esse recurso");

        var quizQuestions = await _questionRepository.GetQuestionsByQuizId(question.QuizId);
        if (quizQuestions.Count == 1 && question.Quiz.IsActive)
            throw new InvalidOperationException("Não é possível deletar a questão pois o Quiz relacionado possui somente uma questão. Desative o Quiz ou adicione outra questão.");

        await _questionRepository.DeleteAsync(question);

        return new ResultDto(new { question.Id });
    }
}