
using Microsoft.Extensions.Configuration;
using QuizDev.Application.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace QuizDev.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string message)
    {
        var from = _configuration["Emails:Sender"] ?? throw new Exception("Não foi possível encontrar o email nas configurações");
        var senderPassword = _configuration["Emails:SenderPassword"] ?? throw new Exception("Não foi possível encontrar o email nas configurações");

        var emailServer = _configuration["Emails:Server"] ?? throw new Exception("Não foi possível encontrar o servidor de email nas configurações");

        var mailMessage = new MailMessage(from, to);
        mailMessage.Subject = subject;
        mailMessage.Body = message;
        
        var smtp = new SmtpClient(emailServer, 587)
        {   
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(from, senderPassword),
            EnableSsl = true
        };


        try
        {
            await smtp.SendMailAsync(mailMessage);
        } catch
        {
            throw new Exception("Não foi possível enviar o e-mail");
        }
    }

}
