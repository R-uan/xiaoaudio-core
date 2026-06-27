using System.Text.Json.Serialization;
using AudioArchive.Modules.Artists.Requests;

namespace AudioArchive.Database.Entity
{
  public class Artist
  {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Nationality { get; set; }
    public string? Note { get; set; }

    public string? BasedAt { get; set; }
    public string? Biography { get; set; }
    public DateTime? Birthday { get; set; }
    
    public bool InActivity { get; set; }
    public DateTime? DebutDate { get; set; }
    public DateTime? GraduationDate { get; set; }

    [JsonIgnore] public int? VerifiedAccountId { get; set; }
    [JsonIgnore] public Account? VerifiedAccount { get; set; }
    [JsonIgnore] public List<Audio>? Audios { get; set; }
    [JsonIgnore] public List<ArtistSocial>? Socials { get; set; }

    public static Artist From(PostArtistRequest request) {
      return new Artist {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Nationality = request.Nationality,
        Note = request.Note,
        BasedAt = request.BasedAt,
        Birthday = request.Birthday,
        Biography = request.Biography,
        InActivity = true,
        DebutDate = request.DebutDate,
        GraduationDate = request.GraduationDate,
      };
    }
  }
}
