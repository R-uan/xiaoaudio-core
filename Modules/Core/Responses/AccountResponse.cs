using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Core.Responses
{
  public class AccountResponse
  {
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }

    public bool VerifiedArtist { get; set; }
    public bool VerifiedAccount { get; set; }

    public Guid? ArtistProfileId { get; set; }
    public string? ArtistName { get; set; }

    public int FavouritesCount { get; set; }
    public int LoginLocationsCount { get; set; }
    public int RequestedTicketsCount { get; set; }

    public List<string> Permissions { get; set; } = [];

    public static AccountResponse From(Account account) {
      return new AccountResponse {
        Id = account.Id,
        Email = account.Email,
        Username = account.Username,
        VerifiedArtist = account.VerifiedArtist,
        VerifiedAccount = account.VerifiedAccount,
        ArtistProfileId = account.ArtistProfileId,
        ArtistName = account.ArtistProfile?.Name,
        FavouritesCount = account.Favourites?.Count ?? 0,
        LoginLocationsCount = account.LoginLocations?.Count ?? 0,
        RequestedTicketsCount = account.RequestedTickets?.Count ?? 0,
        Permissions = account.Permissions.Select(p => p.Name).ToList()
      };
    }
  }
}
