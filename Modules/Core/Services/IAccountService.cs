using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Responses;

namespace AudioArchive.Modules.Core.Services
{
  public interface IAccountService {
    Task<string> AuthenticateAccountAsync(AuthenticationRequest request);
    Task<RegisterAccountResponse> RegisterAccountAsync(RegisterAccountRequest request);
    
    Task<bool> VerifyEmailAvailabilityAsync(string email);
    Task<bool> VerifyUsernameAvailabilityAsync(string username);
    Task PasswordChangeAsync(PasswordChangeRequest request);
  }
}