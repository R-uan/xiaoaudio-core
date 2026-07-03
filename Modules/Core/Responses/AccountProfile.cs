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
    // Following Count are only sent if the profile is an verified artist.
    public int? FollowingCount { get; set; }

    public bool VerifiedArtist { get; set; }
    public string? ArtistName { get; set; }
    public int? FollowersCount { get; set; }

    public static AccountProfile From(Account account, bool canAccessPrivate = false) {
      var profile = new AccountProfile {
        Username = account.Username,
        DisplayName = account.DisplayName ?? account.Username,
        VerifiedArtist = account.VerifiedArtist,
      };

      var preferences = account.Preferences;
      var isPrivate = preferences?.PrivateProfile ?? false;

      if (isPrivate && !canAccessPrivate) {
        return profile;
      }

      if (preferences != null) {
        profile.Mature = preferences.MatureRating;

        if (preferences.PrimaryEmailPublic) {
          profile.Email = account.Email;
        } else {
          profile.Email = account.PublicEmail;
        }

        if (preferences.DisplayBirthday) {
          profile.Birthday = account.Birthday;
        }
      } else {
        profile.Email = account.PublicEmail;
      }

      profile.Biography = account.Biography;

      if (account.VerifiedArtist) {
        profile.FollowingCount = account.Following?.Count;
        profile.ArtistName = account.ArtistProfile?.Name;
        profile.FollowersCount = account.ArtistProfile?.Followers?.Count;
      }

      return profile;
    }
  }
}
