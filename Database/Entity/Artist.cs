using System.Text.Json.Serialization;
using AudioArchive.Modules.Artists.Requests;

namespace AudioArchive.Database.Entity
{
  public class Artist
  {
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required bool InActivity { get; set; } = true;
    public required string Name { get; set; }
    public string? Nationality { get; set; }
    public string? Note { get; set; }

    [JsonIgnore] public int? VerifiedAccountId { get; set; }
    [JsonIgnore] public Account? VerifiedAccount { get; set; }
    [JsonIgnore] public List<Audio>? Audios { get; set; }
    [JsonIgnore] public List<ArtistSocial>? Socials { get; set; }

    public static Artist From(PostArtistRequest request) {
      return new Artist {
        Id = Guid.NewGuid(),
        Name = request.Name,
        InActivity = true
      };
    }
  }
}
