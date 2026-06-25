using AudioArchive.Modules.Audios.Responses.Views;

namespace AudioArchive.Modules.Audios.Responses
{
  public class StoreMultipleAudioResponse
  {
    public required List<AudioView> SavedAudios { get; set; }
    public required List<string> FailedAdditions { get; set; }
    public required List<string> DuplicatedAudios { get; set; }
  }
}
