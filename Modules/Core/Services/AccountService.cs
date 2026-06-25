using AudioArchive.Shared;
using AudioArchive.Database;
using AudioArchive.Infrastructure.Providers;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Responses;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace AudioArchive.Modules.Core.Services
{
  public class AccountService(
    DatabaseContext database,
    IEmailSender emailProvider,
    IHttpContextAccessor httpContext,
    IAuthenticationProvider authProvider
  ) : IAccountService
  {
    private readonly DatabaseContext _db = database;
    private readonly IEmailSender _emailProvider = emailProvider;
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private readonly IAuthenticationProvider _authProvider = authProvider;

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
      var account = await this._db.Accounts
        .Where(a => a.Email == request.Email)
        .FirstOrDefaultAsync() 
        ?? throw new NotFoundException(
          Message: "This email is not associated to an account.",
          Target: "AccountService::AuthenticateAccountAsync"
        );

      if (!account.VerifyPassword(request.Password)) {
        throw new UnauthorizedException(
          Message: "Credentials invalid.",
          Target: "AccountService::AuthenticateAccountAsync"
        );
      }

      return this._authProvider.GenerateToken(account);
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
