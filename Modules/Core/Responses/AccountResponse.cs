using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Core.Responses
{
  public class AccountResponse
  {
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    
    public bool VerifiedArtist { get; set; }
    public bool VerifiedAccount { get; set; }
    
    public static AccountResponse From(Account account) {
      return new AccountResponse {
        Id = account.Id,
        Email = account.Email,
        Username = account.Username,
        DisplayName = account.DisplayName ?? account.Username,
        VerifiedArtist = account.VerifiedArtist,
        VerifiedAccount = account.VerifiedAccount,
      };
    }
  }
}
