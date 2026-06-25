namespace AudioArchive.Modules.Core.Requests
{
  public class RegisterAccountRequest
  {
    public required int AccountId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
  }
}
