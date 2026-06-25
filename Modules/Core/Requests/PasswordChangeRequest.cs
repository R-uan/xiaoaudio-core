namespace AudioArchive.Modules.Core.Requests
{
  public class PasswordChangeRequest
  {
    public required int AccountId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
  }
}
