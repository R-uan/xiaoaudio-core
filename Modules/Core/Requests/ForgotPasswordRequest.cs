namespace AudioArchive.Modules.Core.Requests {
  public class ForgotPasswordRequest {
    public required string Email { get; set; }
    public string? RequesterIP { get; set; }
    public string? Location { get; set; }
    public string? Device { get; set; }
  }
}