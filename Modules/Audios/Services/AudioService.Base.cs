using AudioArchive.Database;
using AudioArchive.Database.Entity;
using AudioArchive.Infrastructure.Identity;
using AudioArchive.Modules.Audios.Requests;
using AudioArchive.Modules.Audios.Responses;

namespace AudioArchive.Modules.Audios.Services
{
  public interface IAudioService
  {
    // Internal
    Task<Audio> UpdateAudioAsync(Guid id, PatchAudioRequest request);
    Task<PostAudioResponse> InsertAudioAsync(PostAudioRequest request);
    // Public
    Task<IEnumerable<AudioResponse>> ListFavouritesAync();
    Task<List<Audio>> AudioQueryAsync(AudioSearchParams parameters);
    // Private
    Task<bool> ToggleFavouriteAudioAsync(Guid audioGuid);
  }
  
  public partial class AudioService(
    DatabaseContext databaseContext,
    ICurrentAccount currentAccout
  ) : IAudioService
  {
    private readonly ICurrentAccount _currentAccount = currentAccout;
    private readonly DatabaseContext _db = databaseContext;
  }
}
