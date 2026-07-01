namespace AudioArchive.Database.Entity {
  public class AccountPreferences {
    public int Id { get; set; }

    public bool MatureRating { get; set; }
    public bool PrivateAudios { get; set; }
    public bool PrivateProfile { get; set; }
    public bool DisplayBirthday { get; set; }
    public bool PrimaryEmailPublic { get; set; }

    public required int AccountId { get; set; }
    public required Account Account { get; set; }
  }
}