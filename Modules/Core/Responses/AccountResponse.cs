using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Core.Responses
{
  public class AccountResponse
  {
    public required Profile Profile { get; set; }
    public required Preferences Preferences { get; set; }

    public static AccountResponse From(Account account) {
      return new AccountResponse {
        Profile = new Profile {
          Id = account.Id,
          Email = account.Email,
          Username = account.Username,
          DisplayName = account.DisplayName ?? account.Username,
          VerifiedArtist = account.VerifiedArtist,
          VerifiedAccount = account.VerifiedAccount,
        },
        Preferences = new Preferences {
          MatureRating = account.Preferences?.MatureRating ?? false,
          PrivateAudios = account.Preferences?.PrivateAudios ?? false,
          PrivateProfile = account.Preferences?.PrivateProfile ?? false,
          DisplayBirthday = account.Preferences?.DisplayBirthday ?? false,
          PrimaryEmailPublic = account.Preferences?.PrimaryEmailPublic ?? false
        }
      };
    }
  }

  public class Profile {
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    
    public bool VerifiedArtist { get; set; }
    public bool VerifiedAccount { get; set; }
  }

  public class Preferences {
    public bool MatureRating { get; set; }
    public bool PrivateAudios { get; set; }
    public bool PrivateProfile { get; set; }
    public bool DisplayBirthday { get; set; }
    public bool PrimaryEmailPublic { get; set; }
  }
}
