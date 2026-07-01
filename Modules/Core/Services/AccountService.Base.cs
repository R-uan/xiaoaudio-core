using AudioArchive.Database;
using AudioArchive.Modules.Core.Requests;
using AudioArchive.Modules.Core.Responses;
using AudioArchive.Infrastructure.Identity;
using AudioArchive.Infrastructure.Providers;

using Microsoft.AspNetCore.Identity.UI.Services;

namespace AudioArchive.Modules.Core.Services
{
  /// <summary>
  /// Core account management service. Handles authentication, registration,
  /// and password lifecycle operations.
  /// </summary>
  /// <remarks>
  /// Dependencies are injected via the primary constructor and stored as
  /// private readonly fields for use across partial class files.
  /// </remarks>
  /// <param name="database">Application database context.</param>
  /// <param name="emailProvider">Service for sending transactional emails.</param>
  /// <param name="currentAccount">Resolves the currently authenticated account.</param>
  /// <param name="authProvider">Generates JWT tokens for authenticated sessions.</param>
  /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
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

  /// <summary>
  /// Contract for account lifecycle operations: registration, authentication,
  /// availability checks, and password management.
  /// </summary>
  public interface IAccountService
  {
    /// <summary>
    /// Creates a new account and returns a JWT token for the authenticated session.
    /// </summary>
    /// <param name="request">Registration payload with email, username, and password.</param>
    /// <returns>JWT token string for the newly created account.</returns>
    Task<string> SignUpAsync(SignUpRequest request);

    /// <summary>
    /// Authenticates an existing account by credentials and returns a JWT token.
    /// </summary>
    /// <param name="request">Authentication payload with email and password.</param>
    /// <returns>JWT token string for the authenticated session.</returns>
    Task<string> SignInAsync(SignInRequest request);

    /// <summary>
    /// Checks whether an email address is available for registration.
    /// </summary>
    /// <param name="email">The email address to verify.</param>
    /// <returns>True if the email is not already in use.</returns>
    Task<bool> VerifyEmailAvailabilityAsync(string email);

    /// <summary>
    /// Checks whether a username is available for registration (not reserved by an artist or taken).
    /// </summary>
    /// <param name="username">The username to verify.</param>
    /// <returns>True if the username is available.</returns>
    Task<bool> VerifyUsernameAvailabilityAsync(string username);

    /// <summary>
    /// Resets the password for the currently authenticated account using a verification code.
    /// </summary>
    /// <param name="req">Payload containing the reset code and new password.</param>
    /// <returns>True if the password was successfully changed.</returns>
    Task<bool> ResetPasswordAsync(ResetPasswordRequest req);

    /// <summary>
    /// Initiates a password reset flow by sending a verification code to the account's email.
    /// </summary>
    /// <param name="req">Payload with the email address for the account to reset.</param>
    /// <returns>True if the account exists and a reset code was sent.</returns>
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest req);

    /// <summary>
    /// Verifies the current account's email. Call with null to request a new
    /// verification email, or with a code to confirm the address.
    /// </summary>
    /// <param name="verificationCode">The verification code, or null to send a new one.</param>
    /// <returns>True if the account was verified, false if an email was sent instead.</returns>
    Task<bool> AccountVerificationAsync(string? verificationCode = null);

    /// <summary>
    /// Retrieves the public profile for an account by username.
    /// </summary>
    /// <param name="username">The username to look up.</param>
    /// <returns>The account profile, or null if not found.</returns>
    Task<AccountProfile?> GetProfileAsync(string username);
  }
}
