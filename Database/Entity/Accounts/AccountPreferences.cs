namespace AudioArchive.Database.Entity {
  public class AccountPreferences {
    public int Id { get; set; }

    // If the artist content is mature
    public bool MatureRating { get; set; }
    // If the artist's audios are private/followers only
    public bool PrivateAudios { get; set; }
    // If the artist's whole profile is private/followers only
    public bool PrivateProfile { get; set; }
    // If the artists birthday is displayed on the profile
    public bool DisplayBirthday { get; set; }
    // If the primary email should be public to others (respect the privateprofile)
    public bool PrimaryEmailPublic { get; set; }

    public required int AccountId { get; set; }
    public required Account Account { get; set; }
  }
}