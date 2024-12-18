using SmartQuiz.Application.DTOs.Questions;
using SmartQuiz.Application.Exceptions;
using SmartQuiz.Application.Services.Interfaces;
using SmartQuiz.Core.Entities;
using SmartQuiz.Core.Repositories;

namespace SmartQuiz.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }
    
    public async Task<Question?> GetByIdAsync(Guid questionId)
    {
        return await _questionRepository.GetByIdAsync(questionId);
    }
    
     public async Task AddAsync(Question question)
    {
       await _questionRepository.AddAsync(question);
    }
    
    public async Task DeleteAsync(Question question)
    {
        await _questionRepository.DeleteAsync(question);
    }

    public async Task UpdateAsync(Question question)
    {
        await _questionRepository.UpdateAsync(question);
    }

    public async Task<IEnumerable<Question>> GetQuestionsByQuizId(Guid quizId)
    {
       return await _questionRepository.GetQuestionsByQuizId(quizId);
    }

    public void AdjustQuestionsOrder(List<Question> questions, Question question)
    {
        if (questions.Any(x => x.Order.Equals(question.Order)))
        {
            //Se já possui, atualiza todas as posições
            var questionsAboveOrder = questions.Where(x => x.Order >= question.Order).ToList();
            questionsAboveOrder.ForEach(x => x.Order++);
        }
        else
        {
            var maxOrder = questions.Any() ? questions.Max(x => x.Order) : -1;
            if (maxOrder == -1)
            {
                question.Order = 0;
            } else if (question.Order > maxOrder)
            {
                question.Order = maxOrder + 1; // Atualizar para ser a última questão
            }
        }
    }

    public Question CreateQuestion(CreateQuestionDto dto)
    {
        var question = new Question
        {
            Text = dto.Text,
            QuizId = dto.QuizId,
            Order = dto.Order,
        };

        question.AnswerOptions = dto.Options.Select(o => new AnswerOption
        {
            QuestionId = question.Id,
            IsCorrectOption = o.IsCorrectOption,
            Response = o.Response
        }).ToList();

        return question;
    }

    public Question UpdateQuestion(Question question, string text)
    {
        question.Text = text;
        
        return question;
    }

    public void RemoveQuestionFromQuiz(Quiz quiz, Question questionToRemove)
    {
        if (quiz.Questions.Count == 1 && questionToRemove.Quiz.IsActive)
            throw new InvalidOperationException("Não é possível deletar a questão pois o Quiz relacionado possui somente uma questão. Desative o Quiz ou adicione outra questão.");
        
        quiz.Questions.Remove(questionToRemove);
        
        foreach (var question in quiz.Questions)
        {
            if (question.Order > questionToRemove.Order)
                question.Order--;
        }
    }

    public void UpdateCorrectOption(Question question, Guid newCorrectOptionId)
    {
        var correctOption = question.AnswerOptions.FirstOrDefault(x => x.IsCorrectOption);
        
        //Remove a opção correta atual
        if (correctOption != null)
            correctOption.IsCorrectOption = false;
        
        //Atualiza a opção correta
        var newCorrectOption = question.AnswerOptions.FirstOrDefault(x => x.Id == newCorrectOptionId);
        if (newCorrectOption == null) 
            throw new NotFoundException("Opção de resposta não encontrada");

        newCorrectOption.IsCorrectOption = true;
    }
}