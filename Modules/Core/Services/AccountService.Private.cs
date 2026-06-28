using AudioArchive.Shared;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using AudioArchive.Infrastructure.Providers;

namespace AudioArchive.Modules.Core.Services
{
  public partial class AccountService
  {
    public async Task<bool> AccountVerificationAsync(string? verificationCode = null) {
      var account = await _currentAccount.GetAsync();
      return verificationCode == null ? 
        await SendVerificationEmailAsync(account) :
        await VerifyCodeAsync(account, verificationCode);
    }

    private async Task<bool> SendVerificationEmailAsync(Account account) {
      var existing = await _db.VerificationCodes
        .Where(v => v.AccountId == account.Id)
        .Where(v => v.Type == CodeType.EmailVerification)
        .FirstOrDefaultAsync();

      var code = existing is not null && !existing.IsExpiredOrUsed()
        ? existing : await CreateVerificationCodeAsync(account);

      var request = _httpContext.HttpContext!.Request;
      var baseUrl = $"{request.Scheme}://{request.Host}";

      await _emailProvider.SendEmailAsync(
        email: account.Email,
        subject: "Verify your account.",
        htmlMessage: EmailProvider.Load("AccountVerificationRequest", new() {
          ["VerificationLink"] = $"{baseUrl}/api/account/verify?code={code.Code}",
          ["AccountUsername"] = account.Username
        })
      );

      return false;
    }

    private async Task<bool> VerifyCodeAsync(Account account, string verificationCode) {
      var request = await _db.VerificationCodes
        .Where(v => v.AccountId == account.Id)
        .Where(v => v.Code == verificationCode)
        .Where(v => v.Type == CodeType.EmailVerification)
        .FirstOrDefaultAsync();

      if (request is null || request.IsExpiredOrUsed()) {
        throw new BadRequestException(
          Message: "Invalid or expired verification code.",
          Target: "AccountService::AccountVerificationAsync"
        );
      }

      request.UseCode();
      account.VerifiedAccount = true;
      await _db.SaveChangesAsync();
      return true;
    }

    private async Task<VerificationCode> CreateVerificationCodeAsync(Account account) {
      var code = VerificationCode.Create(account, CodeType.EmailVerification);
      await _db.VerificationCodes.AddAsync(code);
      await _db.SaveChangesAsync();
      return code;
    }
  }
}
