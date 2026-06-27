namespace AudioArchive.Modules.Artists.Requests
{
  public class PostArtistRequest
  {
    public required string Name { get; set; }
    public string? Nationality { get; set; }
    public string? Note { get; set; }
    public string? BasedAt { get; set; }
    public string? Biography { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? DebutDate { get; set; }
    public DateTime? GraduationDate { get; set; }
  }

  public class PatchArtistRequest
  {
    public string? Name { get; set; }
    public string? Nationality { get; set; }
    public string? Note { get; set; }
    public string? BasedAt { get; set; }
    public string? Biography { get; set; }
    public DateTime? Birthday { get; set; }
    public bool? InActivity { get; set; }
    public DateTime? DebutDate { get; set; }
    public DateTime? GraduationDate { get; set; }
  }
}
