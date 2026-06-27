using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Audios.Responses
{
  public class AudioResponse
  {
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Artist { get; set; }
    public required string Source { get; set; }
    public string? Link { get; set; }
    public bool Local { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    public required DateTime AddedAt { get; set; }

    public required MetadataResponse Metadata { get; set; }

    public static AudioResponse From(Audio audio) {
      return new AudioResponse {
        Id = audio.Id,
        Title = audio.Title,
        Artist = audio.Artist?.Name ?? "Unknown",
        Source = audio.Source,
        Local = audio.Local,
        Link = audio.Link,
        AddedAt = audio.AddedAt,
        UpdatedAt = audio.UpdatedAt,
        Metadata = MetadataResponse.From(audio.Metadata)
      };
    }
  }

}
