using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SmartQuiz.Application.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string ApiUrl;

    public GeminiService(IConfiguration configuration)
    {
        var geminiApiKey = configuration["GeminiApiKey"] ?? throw new Exception("Gemini ApiKey is missing");
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(350);
        ApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={geminiApiKey}";
    }
    
    public async Task<string> RequestGeminiAsync(string prompt)
    {
        // Estrutura do payload para a API
        var payload = new { contents = new object[] { new { parts = new object[]
        {
            new { text = prompt }
        }}}};

        //Requisição para a API do Gemini
        var response = await _httpClient.PostAsJsonAsync(ApiUrl, payload);
        if (response.IsSuccessStatusCode == false)
            throw new InvalidOperationException("Não foi possível concluir a requisição para a API do Gemini");
        
        // Deserializar a resposta da API (com metadados)
        Root geminiContent;
        try
        {
            geminiContent = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync())!;
        }
        catch
        {
            throw new InvalidOperationException("Não foi possível deserializar a resposta");
        }
        var generatedContent = geminiContent.Candidates[0].Content.Parts[0].Text; //Acessar o conteúdo gerado

        // Limpar e ajustar o JSON gerado
        generatedContent = Regex.Replace(generatedContent, @"\p{C}+", ""); 
        generatedContent = Regex.Replace(generatedContent, @"```json", "");
        generatedContent = Regex.Unescape(generatedContent.Trim('`', ' ', '\n', '\r'));

        return generatedContent;
    }
    
    internal record Root(List<Candidate> Candidates);
    internal record Candidate(Content Content);
    internal record Content(List<Part> Parts);
    internal record Part(string Text);
}