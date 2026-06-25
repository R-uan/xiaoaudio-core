using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using AudioArchive.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AudioArchive.Infrastructure.Providers
{
  public class EmailProvider : IEmailSender
  {
    private readonly EmailSettings _settings;

    public EmailProvider(IConfiguration config) {
      _settings = config.GetSection("EmailSettings").Get<EmailSettings>()
          ?? throw new MissingFieldException("EmailSettings not found in configuration.");
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
      message.To.Add(MailboxAddress.Parse(email));
      message.Subject = subject;
      message.Body = new TextPart("html") { Text = htmlMessage };

      using var client = new SmtpClient();
      await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.None);
      await client.SendAsync(message);
      await client.DisconnectAsync(true);
    }
  }
}