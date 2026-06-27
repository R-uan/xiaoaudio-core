namespace AudioArchive.Modules.Audios.Responses
{
  public class PostAudioResponse {
    public required bool Inserted { get; set; }
    public required AudioResponse Audio { get; set; }
  }
}