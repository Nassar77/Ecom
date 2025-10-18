using Ecom.Core.DTO;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Ecom.infrastructure.Reposatries.Service;
public class EmailService : IEmailService
{
    private readonly IConfiguration _Configuration;

    //smtp simple mail transfer protocole
    public EmailService(IConfiguration configuration)
    {
        _Configuration = configuration;
    }
    public async Task SendEmail(EmailDTO emailDTO)
    {
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("Nassar Ecommerce", _Configuration["EmailSetting:From"]));
        message.Subject=emailDTO.Subject;
        message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = emailDTO.Content,
        };
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                await smtp.ConnectAsync(
                    host: _Configuration[key: "EmailSetting:Smtp"],
                    port: int.Parse(s: _Configuration[key: "EmailSetting:Port"]), useSsl: true);

                await smtp.AuthenticateAsync(userName: _Configuration[key: "EmailSetting:Username"],
                    password: _Configuration[key: "EmailSetting:Password"]);

                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                smtp.Disconnect(quit: true);
                smtp.Dispose();
            }
        }
    }
}
