using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Core.Response
{
  public class RegisterAccountResponse
  {
    public required string Email { get; set; }
    public required string Username { get; set; }

    public static RegisterAccountResponse Create(Account account) {
      return new RegisterAccountResponse {
        Email = account.Email,
        Username = account.Username
      };
    }
  }
}
