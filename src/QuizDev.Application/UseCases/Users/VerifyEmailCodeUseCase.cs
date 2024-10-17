
using QuizDev.Application.Exceptions;
using QuizDev.Core.DTOs.Responses;
using QuizDev.Core.Repositories;

namespace QuizDev.Application.UseCases.Users;

public class VerifyEmailCodeUseCase
{
    private readonly IEmailCodeRepository _emailCodeRepository;
    private readonly IUserRepository _userRepository;

    public VerifyEmailCodeUseCase(IEmailCodeRepository emailCodeRepository, IUserRepository userRepository)
    {
        _emailCodeRepository = emailCodeRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultDto> Execute(string code, string email)
    { 
        var emailCode = await _emailCodeRepository.GetByCodeAsync(code);
        if (emailCode == null || emailCode.Email != email || emailCode.Code != code)
        {
            throw new ArgumentException("Código inválido");
        }
    
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        user.EmailIsVerified = true;

        await _userRepository.UpdateAsync(user);

        return new ResultDto(new { Message = "Email verificado com sucesso!" });
    }
}
