using AudioArchive.Shared;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Responses;

namespace AudioArchive.Modules.Core.Services
{
  public partial class AccountService
  {
    public async Task<RegisterAccountResponse> RegisterAccountAsync(RegisterAccountRequest request)
    {
      await VerifyEmailAvailabilityAsync(request.Email);

      var account = new Account
      {
        VerifiedArtist = false,
        Email = request.Email,
        Password = request.Password,
        Username = request.Username,
      };

      await _db.Accounts.AddAsync(account);
      await _db.SaveChangesAsync();

      await SendRegistrationEmailAsync(account);
      return RegisterAccountResponse.Create(account);
    }

    public async Task<string> AuthenticateAccountAsync(AuthenticationRequest request)
    {
      var account = await _db.Accounts
        .Where(a => a.Email == request.Email)
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException(
          Message: "This email is not associated to an account.",
          Target: "AccountService::AuthenticateAccountAsync"
        );

      if (!account.VerifyPassword(request.Password))
      {
        throw new UnauthorizedException(
          Message: "Credentials invalid.",
          Target: "AccountService::AuthenticateAccountAsync"
        );
      }

      return _authProvider.GenerateToken(account);
    }

    public async Task<bool> VerifyEmailAvailabilityAsync(string email)
    {
      var exists = await _db.Accounts.Select(u => u.Email == email).FirstOrDefaultAsync();

      if (exists)
      {
        throw new DuplicatedException(
          Message: "Email is already in use.",
          Target: "AccountService::VerifyEmailAvailability"
        );
      }

      return true;
    }

    public async Task<bool> VerifyUsernameAvailabilityAsync(string username)
    {
      var isReserved = await _db.Artists.Select(a => a.Name == username).FirstOrDefaultAsync();

      if (isReserved)
      {
        throw new ReservedException(
          Message: "Username is reserved. If you think it's for you, contact the support.",
          Target: "AccountService::VerifyUsernameAsync"
        );
      }

      return true;
    }
  }
}
