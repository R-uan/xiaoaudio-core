using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Core.Responses
{
  public class AccountProfile
  {
    public required string DisplayName { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }

    public DateTime? Birthday { get; set; }
    public string? Biography { get; set; }
    public bool Mature { get; set; }
    
    public int? FollowingCount { get; set; }

    public bool VerifiedArtist { get; set; }
    public string? ArtistName { get; set; }
    public int? FollowersCount { get; set; }

    public static AccountProfile From(Account account) {
      var profile = new AccountProfile {
        Username = account.Username,
        DisplayName = account.DisplayName ?? account.Username
      };

      var preferences = account.Preferences;
      profile.Email = preferences.PrimaryEmailPublic ?
        account.Email : account.PublicEmail;
        
        var verifiedArtist = account.VerifiedArtist;
      var followersCount = verifiedArtist ? account.ArtistProfile?.Followers.Count : 0;

      return profile;
    }
  }
}
