using AudioArchive.Database;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Infrastructure.Identity;
using AudioArchive.Infrastructure.Providers;

using Microsoft.AspNetCore.Identity.UI.Services;

namespace AudioArchive.Modules.Core.Services
{
  public partial class AccountService(
    DatabaseContext database,
    IEmailSender emailProvider,
    ICurrentAccount currentAccount,
    IAuthenticationProvider authProvider,
    IHttpContextAccessor httpContextAccessor
  ) : IAccountService
  {
    private readonly DatabaseContext _db = database;
    private readonly IEmailSender _emailProvider = emailProvider;
    private readonly ICurrentAccount _currentAccount = currentAccount;
    private readonly IAuthenticationProvider _authProvider = authProvider;
    private readonly IHttpContextAccessor _httpContext = httpContextAccessor;
  }
  
  public interface IAccountService
  {
    Task<string> SignUpAsync(SignUpRequest request);
    Task<string> SignInAsync(SignInRequest request);
    
    Task<bool> VerifyEmailAvailabilityAsync(string email);
    Task<bool> VerifyUsernameAvailabilityAsync(string username);
    
    Task<bool> ResetPasswordAsync(ResetPasswordRequest req);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest req);
  }
}
