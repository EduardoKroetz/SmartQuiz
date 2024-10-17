
using QuizDev.Application.Services.Interfaces;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Entities;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class VerifyEmailUseCase
{
    private readonly IEmailService _emailService;
    private readonly IEmailCodeRepository _emailCodeRepository;

    public VerifyEmailUseCase(IEmailService emailService, IEmailCodeRepository emailCodeRepository)
    {
        _emailService = emailService;
        _emailCodeRepository = emailCodeRepository;
    }

    public async Task<ResultDto> Execute(string email)
    {
        var existsEmailCode = await _emailCodeRepository.GetByEmailAsync(email);
        if (existsEmailCode != null)
        {
            await _emailCodeRepository.DeleteAsync(existsEmailCode);
        }

        var emailCode = new EmailCode(email);

        await _emailCodeRepository.CreateAsync(emailCode);

        await _emailService.SendEmailAsync(email, "[QuizDev] Confirme seu endereço de E-mail", $"Seu código de Autenticação de Dois Fatores: {emailCode.Code}");

        return new ResultDto(new { });
    }
}
