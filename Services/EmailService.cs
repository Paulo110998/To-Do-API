using System.Net.Mail;
using System.Net;

namespace TO_DO___API.Services;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587; // Porta do servidor SMTP
    private readonly string _smtpUsername = "noreply@nyame.app";
    private readonly string _smtpPassword = "bqyurxjbsmrqjmdj";

    // ENVIO DE EMAIL "BEM-VINDO" !
    public async Task WelcomeEmail(string destinatario, string nomeUsuario)
    {
        // Configurando SMPT
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true,
        };

        // Mensagem do corpo do email
        var mensagem = new MailMessage
        {
            From = new MailAddress(_smtpUsername),
            Subject = "Bem-vindo ao NextCash.",
            Body = $"Olá {nomeUsuario},\n\nBem-vindo ao Nyame! Agradecemos por se cadastrar!",
            IsBodyHtml = false,
        };

        // enviando email para o destinatário
        mensagem.To.Add(destinatario);

        await smtpClient.SendMailAsync(mensagem);
    }

    // ENVIO DE EMAIL PARA RESETAR A SENHA
    public async Task PasswordResetEmail(string destinatario, string token)
    {
        // Configurando SMPT
        var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = false,
        };

        // Link para acesso de reset por token
        // var resetUrl = $"https://api2.stepone.com.br/reset-password/?token={WebUtility.UrlEncode(token)}";
        var resetUrl = $"https://next-cash-front-end.vercel.app/resetar-senha/{WebUtility.UrlEncode(token)}";


        // Mensagem do corpo do email
        var mensagem = new MailMessage
        {
            From = new MailAddress(_smtpUsername),
            Subject = "Recuperação de Senha - NextCash",
            Body = $"Olá,\n\nVocê solicitou a recuperação de senha para a sua conta no Nyame. Clique no link a seguir para redefinir sua senha:\n{resetUrl}\n\nSe você não solicitou esta recuperação, ignore este e-mail.",
            IsBodyHtml = false,
        };

        // enviando email para o destinatário
        mensagem.To.Add(destinatario);

        await smtpClient.SendMailAsync(mensagem);


    }
}