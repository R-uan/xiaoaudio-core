using AudioArchive.Shared;
using AudioArchive.Database.Entity;
using AudioArchive.Modules.Core.Requests;

namespace AudioArchive.Modules.Core.Services
{
  public partial class AccountService
  {
    public async Task PasswordChangeAsync(PasswordChangeRequest request)
    {
      var currentUser = await _currentAccount.GetAsync();

      if (!currentUser.VerifyPassword(request.OldPassword))
      {
        throw new UnauthorizedException(
          Message: "You do not have permission to do that.",
          Target: "AccountService::PasswordChangeAsync"
        );
      }

      currentUser.ChangePassword(request.NewPassword);
      await _db.SaveChangesAsync();
    }

    private async Task SendRegistrationEmailAsync(Account account)
    {
      var verificationCode = VerificationCode.Create(
        account: account,
        type: CodeType.EmailVerification
      );

      await _emailProvider.SendEmailAsync(
        email: account.Email,
        subject: "Verify your newly created account.",
        htmlMessage: "Email Body"
      );
    }
  }
}
