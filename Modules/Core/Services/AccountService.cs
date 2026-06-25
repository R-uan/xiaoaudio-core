using AudioArchive.Shared;
using AudioArchive.Database;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Core.Request;
using AudioArchive.Modules.Core.Response;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AudioArchive.Modules.Core.Services
{
  public class AccountService(
    IHttpContextAccessor httpContext,
    DatabaseContext database,
    IEmailSender emailProvider
  )
  {
    private readonly DatabaseContext _db = database;
    private readonly IEmailSender _emailProvider = emailProvider;
    private readonly IHttpContextAccessor _httpContext = httpContext;

    private Account CurrentUser => _httpContext.HttpContext?.Items["CurrentUser"] as Account
      ?? throw new UnauthorizedAccessException("Not authenticated");

    public async Task<RegisterAccountResponse> RegisterAccountAsync(RegisterAccountRequest request) {
      await this.VerifyEmailAvailabilityAsync(request.Email);

      var account = new Account {
        VerifiedArtist = false,
        Email = request.Email,
        Password = request.Password,
        Username = request.Username,
      };

      await this._db.Accounts.AddAsync(account);
      await this._db.SaveChangesAsync();

      await this.SendRegistrationEmailAsync(account);
      return RegisterAccountResponse.Create(account);
    }

    public async Task<string> AuthenticateAccountAsync(AuthenticationRequest request) {
      
    }
    
    public async Task PasswordChangeAsync(PasswordChangeRequest request) {
      if (!this.CurrentUser.VerifyPassword(request.OldPassword)) {
        throw new UnauthorizedException(
          Message: "You do not have permission to do that.",
          Target: "AccountService::PasswordChangeAsync"
        );
      }

      this.CurrentUser.ChangePassword(request.NewPassword);
      await this._db.SaveChangesAsync();
    }

    private async Task SendRegistrationEmailAsync(Account account) {
      var verificationCode = VerificationCode.Create(
        account: account,
        type: CodeType.EmailVerification
      );

      await this._emailProvider.SendEmailAsync(
        email: account.Email,
        subject: "Verify your newly created account.",
        htmlMessage: "Email Body"
      );
    }

    public async Task<bool> VerifyEmailAvailabilityAsync(string email) {
      // Validate Email Format

      var exists = await this._db.Accounts.Select(u => u.Email == email).FirstOrDefaultAsync();

      if (exists) {
        throw new DuplicatedException(
          Message: "Email is already in use.",
          Target: "AccountService::VerifyEmailAvailability"
        );
      }

      return true;
    }

    public async Task<bool> VerifyUsernameAvailabilityAsync(string username) {
      var isReserved = await this._db.Artists.Select(a => a.Name == username).FirstOrDefaultAsync();

      if (isReserved) throw new ReservedException(
        Message: "Username is reserved. If you think it's for you, contact the support.",
        Target: "AccountService::VerifyUsernameAsync"
      );

      // Other verifications

      return true;
    }
  }
}
