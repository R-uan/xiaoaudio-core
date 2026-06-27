using System.Security.Cryptography;

namespace AudioArchive.Database.Entity
{
  public class VerificationCode
  {
    public int Id { get; set; }
    public bool Used { get; set; }
    public required string Code { get; set; }
    public required CodeType Type { get; set; }

    public required int AccountId { get; set; }
    public required Account Account { get; set; }

    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public static VerificationCode Create(Account account, CodeType type) {
      return new VerificationCode {
        Account = account,
        AccountId = account.Id,
        Code = GenerateCode(type),
        CreatedAt = DateTime.UtcNow,
        ExpiresAt = DateTime.UtcNow.Add(GetExpiration(type)),
        Used = false,
        Type = type
      };
    }

    private static TimeSpan GetExpiration(CodeType type) => type switch {
      CodeType.EmailVerification => TimeSpan.FromHours(24),
      CodeType.PasswordReset => TimeSpan.FromMinutes(15),
      CodeType.LoginOTP => TimeSpan.FromMinutes(5),
      _ => throw new ArgumentOutOfRangeException(nameof(type), "Unknown code type")
    };

    private static string GenerateCode(CodeType type) => type switch {
      CodeType.LoginOTP => Random.Shared.Next(100000, 999999).ToString(),
      _ => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
    };
  }

  public enum CodeType
  {
    EmailVerification,
    PasswordReset,
    LoginOTP
  }
}
