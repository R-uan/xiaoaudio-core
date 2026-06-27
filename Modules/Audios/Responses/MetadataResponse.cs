using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Audios.Responses
{
  public class MetadataResponse
  {
    public int? ReleaseYear { get; set; }
    public string? Genre { get; set; }
    public int? Duration { get; set; }

    public List<string>? Tags { get; set; }

    public static MetadataResponse From(AudioMetadata metadata) {
      return new MetadataResponse {
        Genre = metadata.Genre,
        Duration = metadata.Duration,
        ReleaseYear = metadata.ReleaseYear,
        Tags = metadata.Tags?.Select(a => a.Name).Order().ToList()
      };
    }
  }
}
