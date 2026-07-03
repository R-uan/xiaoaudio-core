using AudioArchive.Shared;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Responses;
using AudioArchive.Infrastructure.Providers;

namespace AudioArchive.Modules.Core.Services
{
  public partial class AccountService
  {
    public async Task<(AccountResponse, string)> SignUpAsync(SignUpRequest req) {
      await VerifyUsernameAvailabilityAsync(req.Username);
      await VerifyEmailAvailabilityAsync(req.Email);
      
      var account = new Account {
        VerifiedArtist = false,
        Email = req.Email,
        Username = req.Username,
        DisplayName = req.DisplayName ?? req.Username,
        Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
      };

      var preferences = new AccountPreferences() {
        AccountId = account.Id,
        Account = account,
      };

      await _db.Accounts.AddAsync(account);
      await _db.AccountPreferences.AddAsync(preferences);
      await _db.SaveChangesAsync();
      // TODO: Send email verification email
      return (
        AccountResponse.From(account), 
        _authProvider.GenerateToken(account)
      );
    }

    public async Task<(AccountResponse, string)> SignInAsync(SignInRequest req) {
      var account = await _db.Accounts
        .Include(a => a.ArtistProfile)
        .Include(a => a.Following)
        .Include(a => a.Preferences)
        .AsSplitQuery()
        .FirstOrDefaultAsync(a => a.Email == req.Email) ?? 
          throw new NotFoundException(
            Message: "Profile not found",
            Target: "AccountService::GetProfileAsync"
          );

      if (!account.VerifyPassword(req.Password)) {
        throw new UnauthorizedException(
          Message: "Credentials invalid.",
          Target: "AccountService::AuthenticateAccountAsync"
        );
      }

      return (
        AccountResponse.From(account), 
        _authProvider.GenerateToken(account)
      );
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest req) {
      var account = await this._db.Accounts.Where(a => a.Email == req.Email).FirstOrDefaultAsync() ??
        throw new NotFoundException(
          Message: "This account do not exist",
          Target: "AccountService::ForgotPasswordRequestAsync"
        );

      var verificationCode = VerificationCode.Create(account, CodeType.PasswordReset);
      // TODO: This url should aim to the front-end with the code
      // as we don't have the front-end yet then this placeholder will serve.
      var request = _httpContext.HttpContext!.Request;
      var baseUrl = $"{request.Scheme}://{request.Host}";
      
      var htmlMessageTemplate = EmailProvider.Load("ForgotPasswordRequest", new() {
        ["ResetPasswordLink"] = $"{baseUrl}/api/account/password-reset/${verificationCode.Code}",
        ["AccountUsername"] = account.Username,
      });

      await _emailProvider.SendEmailAsync(
        subject: "Forgot password request.",
        htmlMessage: htmlMessageTemplate,
        email: account.Email
      );

      return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest req) {
      var account = await _currentAccount.GetAsync();
      var verificationCode = await this._db.VerificationCodes
        .Where(a => a.Code == req.VerificationCode)
        .Where(a => a.AccountId == account.Id)
        .FirstOrDefaultAsync();

      if (verificationCode == null) {
        throw new NotFoundException(
          Message: "No password change request was found.",
          Target: "AccountService.Private::ResetPasswordAsync"
        );
      }

      if (verificationCode.ExpiresAt > DateTime.UtcNow) {
        throw new ExpiredException(
          Message: "The verification code of this request has expired.",
          Target: "AccountService.Private::ResetPasswordAsync"
        );
      }

      if (verificationCode.Used == true) {
        throw new ExpiredException(
          Message: "This request has already been fulfilled.",
          Target: "AccountService.Private::ResetPasswordAsync"
        );
      }

      account.ChangePassword(req.NewPassword);
      verificationCode.UseCode();

      await this._db.SaveChangesAsync();
      
      var htmlMessageTemplate = EmailProvider.Load("ResetPasswordResponse", new() {
        ["AccountUsername"] = account.Username,
      });

      await _emailProvider.SendEmailAsync(
        subject: "Your password has been changed successfully",
        htmlMessage: htmlMessageTemplate,
        email: account.Email
      );

      return true;
    }

    public async Task<bool> VerifyEmailAvailabilityAsync(string email) {
      var exists = await _db.Accounts.AnyAsync(u => u.Email == email);
      if (exists) {
        throw new DuplicatedException(
          Message: "Email is already in use.",
          Target: "AccountService::VerifyEmailAvailability"
        );
      }
      return true;
    }

    public async Task<bool> VerifyUsernameAvailabilityAsync(string username) {
      if (await _db.Artists.AnyAsync(a => a.Name == username)) {
        throw new ConflictException(
          Message: "Username is already in use. If you think it's reserved for you you, contact the support.",
          Target: "AccountService::VerifyUsernameAsync"
        );
      }
      return true;
    }

    public async Task<AccountProfile?> GetProfileAsync(string username) {
      var account = await _db.Accounts
        .Include(a => a.ArtistProfile)
        .Include(a => a.Favourites)
        .Include(a => a.Following)
        .Include(a => a.Preferences)
        .AsSplitQuery()
        .FirstOrDefaultAsync(a => a.Username == username) ?? 
          throw new NotFoundException(
            Message: "Profile not found",
            Target: "AccountService::GetProfileAsync"
          );

      if (account.ArtistProfile != null) {
        await _db.Entry(account.ArtistProfile)
          .Collection(a => a.Followers)
          .LoadAsync();
      }

      bool canAccessPrivate = false;
      var isPrivate = account.Preferences?.PrivateProfile ?? false;

      if (isPrivate) {
        var currentUser = await _currentAccount.GetAsync();
        if (currentUser.Id == account.Id) {
          canAccessPrivate = true;
        } else if (account.VerifiedArtist && account.ArtistProfileId != null) {
          canAccessPrivate = await _db.Accounts
            .AnyAsync(
              a => a.Id == currentUser.Id &&
              a.Following.Any(art => art.Id == account.ArtistProfileId)
            );
        }
      }

      return AccountProfile.From(account, canAccessPrivate);
    }
  }
}
