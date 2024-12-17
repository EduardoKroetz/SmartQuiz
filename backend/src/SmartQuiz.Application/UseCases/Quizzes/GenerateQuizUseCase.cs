using Newtonsoft.Json;
using SmartQuiz.Application.DTOs.Records;
using SmartQuiz.Application.DTOs.AnswerOptions;
using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.DTOs.Quizzes;
using SmartQuiz.Application.DTOs.Responses;
using SmartQuiz.Application.Services.Interfaces;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GenerateQuizUseCase
{
    private readonly IQuestionService _questionService;
    private readonly IQuizService _quizService;
    private readonly IGeminiService _geminiService;


    public GenerateQuizUseCase(IQuestionService questionService, IQuizService quizService, IGeminiService geminiService)
    {
        _questionService = questionService;
        _quizService = quizService;
        _geminiService = geminiService;
    }

    public async Task<ResultDto<IdResult>> Execute(GenerateQuizDto generateQuizDto, Guid userId)
    {
        var quizPrompt = @$"
                Crie um Quiz de múltipla escolha sobre o tema '{generateQuizDto.Theme}' com {generateQuizDto.NumberOfQuestions} questões de dificuldade '{generateQuizDto.Difficulty}'. 
                Sem nenhum tipo de formatação markdown, somente JSON em formato de string. Ex:
                {{
                    ""title"": ""Título do Quiz com no máximo 50 caracteres"",
                    ""description"": ""Descrição do Quiz com no mínimo 45 caracteres e no máximo 100 caracteres"", 
                    ""questions"": [
                        {{
                            ""text"": ""Texto da pergunta aqui"",
                            ""order"": 0,
                            ""answerOptions"": [
                                {{ ""Response"": ""Opção 1"", ""isCorrectOption"": false }},
                                {{ ""Response"": ""Opção 2"", ""isCorrectOption"": true }},
                                {{ ""Response"": ""Opção 3"", ""isCorrectOption"": false }},
                                {{ ""Response"": ""Opção 4"", ""isCorrectOption"": false }}
                            ]
                        }},
                        ...
                    ]
                }}
        ";
        
        var generatedContent = await _geminiService.RequestGeminiAsync(quizPrompt);
        
        //Deserializar o texto Json
        QuizResponse quizResponse;
        try
        {
            quizResponse = JsonConvert.DeserializeObject<QuizResponse>(generatedContent)!;
        }
        catch
        {
            throw new ArgumentException("O conteúdo do JSON gerado não pôde ser deserializado corretamente.");
        }
        
        //Criar o quiz com base nas informações da resposta
        var editorQuizDto = new EditorQuizDto
        {
            Title = quizResponse.Title,
            Description = quizResponse.Description,
            Expires = generateQuizDto.Expires,
            ExpiresInSeconds = generateQuizDto.ExpiresInSeconds,
            Difficulty = generateQuizDto.Difficulty,
            Theme = generateQuizDto.Theme
        };
        
        //Criar o Quiz
        var quiz = _quizService.CreateQuiz(editorQuizDto, userId);
        await _quizService.AddAsync(quiz);

        //Criar as questões para o quiz
        foreach (var questionResponse in quizResponse.Questions)
        {
            //Converter DTOs
            var optionsDto = questionResponse.AnswerOptions
                .Select(answerOption => new CreateAnswerOptionDto
                {
                    Response = answerOption.Response,
                    IsCorrectOption = answerOption.IsCorrectOption,
                })
                .ToList();

            //Criar a questão
            var questionDto = new CreateQuestionDto
            {
                QuizId = quiz.Id,
                Order = questionResponse.Order,
                Text = questionResponse.Text,
                Options = optionsDto
            };

            var question = _questionService.CreateQuestion(questionDto);
            await _questionService.AddAsync(question);
        }

        //Ativar o quiz
        _quizService.ToggleQuiz(quiz);
        await _quizService.UpdateAsync(quiz);

        return new ResultDto<IdResult>(new IdResult(quiz.Id));
    }
}

internal record QuizResponse(string Title, string Description, List<QuizQuestionsResponse> Questions);
internal record QuizQuestionsResponse(string Text, int Order, List<QuestionAnswerOptionResponse> AnswerOptions);
internal record QuestionAnswerOptionResponse(string Response, bool IsCorrectOption);