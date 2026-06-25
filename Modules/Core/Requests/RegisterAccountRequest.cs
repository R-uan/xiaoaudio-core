namespace AudioArchive.Modules.Core.Requests
{
  public class RegisterAccountRequest
  {
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
  }
}
