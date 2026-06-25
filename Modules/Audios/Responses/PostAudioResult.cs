using AudioArchive.Modules.Audios.Responses.Views;

namespace AudioArchive.Modules.Audios.Responses
{
  public record PostAudioResult
  {
    public required bool IsNew { get; init; }  
    public required AudioView Audio { get; init; }
  }
}