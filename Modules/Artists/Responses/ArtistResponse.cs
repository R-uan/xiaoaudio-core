using AudioArchive.Database.Entity;

namespace AudioArchive.Modules.Artists.Responses
{
  public record TagFrequency(string Name, int Count);

  public class ArtistResponse
  {
    public required string Name { get; set; }
    public string? Nationality { get; set; }
    public string? Note { get; set; }
    public string? BasedAt { get; set; }
    public string? Biography { get; set; }
    public DateTime? Birthday { get; set; }

    public bool InActivity { get; set; }
    public DateTime? DebutDate { get; set; }
    public DateTime? GraduationDate { get; set; }

    public int AudioCount { get; set; }
    public List<TagFrequency>? MostFrequentTags { get; set; }

    public static ArtistResponse From(Artist artist) {
      return new ArtistResponse {
        Name = artist.Name,
        Nationality = artist.Nationality,
        Note = artist.Note,
        BasedAt = artist.BasedAt,
        Biography = artist.Biography,
        Birthday = artist.Birthday,
        InActivity = artist.InActivity,
        DebutDate = artist.DebutDate,
        GraduationDate = artist.GraduationDate,
        AudioCount = artist.Audios?.Count ?? 0,
        MostFrequentTags = artist.Audios?.SelectMany(audio => audio.Metadata.Tags)
          .GroupBy(tag => tag.Name)
          .OrderByDescending(group => group.Count())
          .Take(3)
          .Select(group => new TagFrequency(group.Key, group.Count()))
          .ToList()
      };
    }
  }
}
