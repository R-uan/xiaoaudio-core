namespace AudioArchive.Modules.Playlists.Responses
{
  public class PlaylistResponse
  {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Guid>? Audios { get; set; }
  }
}
