using AudioArchive.Database.Entity;
using AudioArchive.Modules.Audios.Requests;
using AudioArchive.Modules.Audios.Responses;

namespace AudioArchive.Modules.Audios.Services
{
  public interface IAudioService
  {
    Task<PostAudioResult> StoreAudio(PostAudioRequest request);
    Task<List<Tag>> ProcessTags(List<string> targetTags);
    Task<Audio> UpdateAudio(Guid id, PatchAudioRequest request);
    Task<List<Audio>> QueryAudios(AudioSearchParams parameters);
  }
}
