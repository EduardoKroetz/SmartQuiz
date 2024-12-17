using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Questions;

public class CreateQuestionUseCase
{
    private readonly IQuizService _quizService;
    private readonly IQuestionService _questionService;
    private readonly IAnswerOptionService _answerOptionService;
    private readonly IAuthService _authService;
    
    public CreateQuestionUseCase(IQuizService quizService, IQuestionService questionService, IAnswerOptionService answerOptionService, IAuthService authService)
    {
        _quizService = quizService;
        _questionService = questionService;
        _answerOptionService = answerOptionService;
        _authService = authService;
    }

    public async Task<ResultDto> Execute(CreateQuestionDto createQuestionDto, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(createQuestionDto.QuizId);
        if (quiz is null) 
            throw new NotFoundException("Quiz não encontrado");

        _authService.ValidateSameUser(quiz.UserId, userId);
        
        //Criar nova questão
        var question = _questionService.CreateQuestion(createQuestionDto);
        
        // Atualizar a ordem das questões
        _questionService.AdjustQuestionsOrder(quiz.Questions, question);
        
        // Salvar questão no banco de dados
        await _questionService.AddAsync(question);

        foreach (var createAnswerOption in createQuestionDto.Options)
        {
            var answerOption = _answerOptionService.CreateAnswerOption(createAnswerOption);
            await _answerOptionService.AddAsync(answerOption);
        }

        return new ResultDto(new { question.Id });
    }
}