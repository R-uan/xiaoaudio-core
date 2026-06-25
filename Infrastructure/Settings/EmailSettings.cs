namespace AudioArchive.Infrastructure.Settings {
  public class EmailSettings
  {
    public int Port { get; set; }
    public required string Host { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
  }
}