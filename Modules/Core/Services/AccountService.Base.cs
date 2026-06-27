using AudioArchive.Database;
using AudioArchive.Infrastructure.Providers;
using AudioArchive.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Responses;

namespace AudioArchive.Modules.Core.Services
{
  public interface IAccountService
  {
    Task<string> AuthenticateAccountAsync(AuthenticationRequest request);
    Task<RegisterAccountResponse> RegisterAccountAsync(RegisterAccountRequest request);
    Task<bool> VerifyEmailAvailabilityAsync(string email);
    Task<bool> VerifyUsernameAvailabilityAsync(string username);
    Task PasswordChangeAsync(PasswordChangeRequest request);
  }

  public partial class AccountService(
    DatabaseContext database,
    IEmailSender emailProvider,
    ICurrentAccount currentAccount,
    IAuthenticationProvider authProvider
  ) : IAccountService
  {
    private readonly DatabaseContext _db = database;
    private readonly IEmailSender _emailProvider = emailProvider;
    private readonly ICurrentAccount _currentAccount = currentAccount;
    private readonly IAuthenticationProvider _authProvider = authProvider;
  }
}
