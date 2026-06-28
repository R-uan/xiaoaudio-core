namespace AudioArchive.Modules.Core.Requests {
  public class SignInRequest {
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? RequesterIP { get; set; }
    public string? Location { get; set; }
    public string? Device { get; set; }
  }
}