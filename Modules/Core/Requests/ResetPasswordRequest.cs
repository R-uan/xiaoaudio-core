namespace AudioArchive.Modules.Core.Requests
{
  public class ResetPasswordRequest
  {
    public required string NewPassword { get; set; }
    public string? VerificationCode { get; set; }
  }
}
