using Microsoft.AspNetCore.Identity.UI.Services;
namespace AudioArchive.Infrastructure.Providers
{
  public class EmailProvider : IEmailSender
  {
    public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
      throw new NotImplementedException();
    }
  }
}
