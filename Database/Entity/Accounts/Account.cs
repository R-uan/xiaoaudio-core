using AudioArchive.Shared;

namespace AudioArchive.Database.Entity
{
  public class Account
  {
    public int Id { get; set; }
    
    public required string Email { get; set; }
    public string? PublicEmail { get; set; }
    
    public required string Username { get; set; }
    public required string Password { get; set; }
    
    public string? DisplayName { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Biography { get; set; }

    public bool VerifiedArtist { get; set; } = false;
    public bool VerifiedAccount { get; set; } = false;
    
    public Guid? ArtistProfileId { get; set; }
    public Artist? ArtistProfile { get; set; }

    public AccountPreferences? Preferences { get; set; }
    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<LoginLocation>? LoginLocations { get; set; } = [];
    
    public ICollection<Audio> Favourites { get; set; } = [];  
    public ICollection<Artist> Following { get; set; } = [];
    
    public ICollection<SupportTicket>? AssignedTickets { get; set; } = [];
    public ICollection<SupportTicket>? RequestedTickets { get; set; } = [];
    public ICollection<SupportTicketMessage>? TicketMessages { get; set; } = [];


    public bool VerifyPassword(string password) {
      // TODO: password hashing
      return this.Password == password;
    }

    public bool ChangePassword(string newPassword) {
      // 1. Validate the password
      // 2. Hash it
      this.Password = newPassword;
      return true;
    }

    public bool ChangeEmail(string email) {
      this.Email = email;
      return true;
    }

    public async Task UnverifyArtist() {
      if (!this.VerifiedArtist) {
        throw new VerificationException(
          Message: "The user is not verified.",
          Target: "User::VerifyArtist::31"
        );
      }

      this.ArtistProfile = null;
      this.ArtistProfileId = null;
    }

    public async Task VerifyArtist(Artist artist) {
      if (this.VerifiedArtist || artist.VerifiedAccount != null) {
        throw new VerificationException(
          Message: "The user or artist is already verified. Check the manual to see how to proceed",
          Target: "User::VerifyArtist::31"
        );
      }

      this.ArtistProfile = artist;
      this.ArtistProfileId = artist.Id;
    }

    public void AddFavourite(Audio audio) {
      (this.Favourites ??= []).Add(audio);
    }
    
    public void RemoveFavourite(Audio audio) {
      (this.Favourites)?.Remove(audio);
    }
  }
}
