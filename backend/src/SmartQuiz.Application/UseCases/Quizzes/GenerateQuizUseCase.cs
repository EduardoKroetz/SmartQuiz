using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartQuiz.Application.Common;
using SmartQuiz.Application.UseCases.Questions;
using SmartQuiz.Core.DTOs.AnswerOptions;
using SmartQuiz.Core.DTOs.Questions;
using SmartQuiz.Core.DTOs.Quizzes;
using SmartQuiz.Core.DTOs.Responses;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.UseCases.Quizzes;

public class GenerateQuizUseCase
{
    public GenerateQuizUseCase(IQuizRepository quizRepository ,IConfiguration configuration, CreateQuizUseCase createQuizUseCase, ToggleQuizUseCase toggleQuizUseCase, CreateQuestionUseCase createQuestionUseCase)
    {
        _createQuizUseCase = createQuizUseCase;
        _toggleQuizUseCase = toggleQuizUseCase;
        _createQuestionUseCase = createQuestionUseCase;
        _createQuizUseCase = createQuizUseCase;
        _quizRepository = quizRepository;
        var geminiApiKey = configuration["GeminiApiKey"] ?? throw new Exception("Gemini ApiKey is missing");
        _httpClient = new HttpClient();
        ApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={geminiApiKey}";
    }

    private readonly string ApiUrl;
    private readonly HttpClient _httpClient;
    private readonly CreateQuizUseCase _createQuizUseCase;
    private readonly ToggleQuizUseCase _toggleQuizUseCase;
    private readonly CreateQuestionUseCase _createQuestionUseCase;
    private readonly IQuizRepository _quizRepository;
    
    public async Task<ResultDto<IdResult>> Execute(GenerateQuizDto generateQuizDto, Guid userId)
    {
        var prompt = @$"
                Crie um Quiz de múltipla escolha sobre o tema '{generateQuizDto.Theme}' com {generateQuizDto.NumberOfQuestions} questões na dificuldade '{generateQuizDto.Difficulty}'. 
                Sem nenhum tipo de formatação markdown, somente JSON em formato de string. Ex:
                {{
                    ""title"": ""Título do Quiz"",
                    ""description"": ""Descrição curta do Quiz"", 
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
        
        var payload = new
        {
           contents = new object[]
           {
               new
               {
                   parts = new object[]
                   {
                       new { text = prompt }
                   }
               }
           }
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(payload),
            Encoding.UTF8,
            "application/json"
        );
        
        //Requisição para o Gemini
        var response = await _httpClient.PostAsync(ApiUrl, content);
        if (response.IsSuccessStatusCode == false)
        {
            var contentRes = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException("Não foi possível concluir a requisição para a API do Gemini");
        }

        // Deserializar a resposta completa com metadados
        var responseContentString = await response.Content.ReadAsStringAsync();
        var geminiContent = JsonConvert.DeserializeObject<dynamic>(responseContentString);
        if (geminiContent == null)
        {
            throw new InvalidOperationException("Dados em formato inválido");
        }
        
        //Pegar somente o texto gerado e converter para um formato json válido
        var escapedJsonString = geminiContent.candidates[0].content.parts[0].text;
        var jsonString = (string) Regex.Replace(escapedJsonString.ToString(), @"\\", string.Empty);
        jsonString = jsonString.Trim();
        jsonString = jsonString.Substring(1, jsonString.Length - 2);
        
        // Deserializar o conteúdo JSON
        var quizResponse = JsonConvert.DeserializeObject<QuizResponse>(jsonString);
        if (quizResponse == null)
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
        var quizResultDto = await _createQuizUseCase.Execute(editorQuizDto, userId);
        var quizId = quizResultDto.Data!.Id;
        
        //Criar as questões para o quiz
        foreach (var questionResponse in quizResponse.Questions)
        {
            //Criar as opções para cada questão
            var optionsDto = new List<CreateAnswerOptionInQuestionDto>();
            foreach (var answerOption in questionResponse.AnswerOptions)
            {
                var answerOptionDto = new CreateAnswerOptionInQuestionDto
                {
                    Response = answerOption.Response,
                    IsCorrectOption = answerOption.IsCorrectOption
                };
                optionsDto.Add(answerOptionDto);
            }
        
            //Criar a questão
            var createQuestionDto = new CreateQuestionDto
            {
                QuizId = quizId,
                Order = questionResponse.Order,
                Text = questionResponse.Text,
                Options = optionsDto
            };
        
            await _createQuestionUseCase.Execute(createQuestionDto, userId);
        }

        //Ativar o quiz
        await _toggleQuizUseCase.Execute(quizId, userId);
        
        return new ResultDto<IdResult>(new IdResult(quizResultDto.Data!.Id));
    }
}

public record QuizResponse(string Title, string Description, List<QuizQuestionsResponse> Questions);

public record QuizQuestionsResponse(string Text, int Order, List<QuestionAnswerOptionResponse> AnswerOptions);

public record QuestionAnswerOptionResponse(string Response, bool IsCorrectOption);