using AudioArchive.Shared;
using AudioArchive.Modules.Audios.Responses;

namespace AudioArchive.Modules.Audios.Services
{
  public partial class AudioService : IAudioService
  {
    public async Task<bool> ToggleFavouriteAudioAsync(Guid audioGuid) {
      var account = await this._currentAccount.GetAsync()!;

      var audio = await this._db.Audios.FindAsync(audioGuid) ??
        throw new NotFoundException(
          Message: "Audio was not found.",
          Target: "AudioService::FavouriteAudioAsync"
        );
        
      var favourited = false;
      if (account.Favourites.Any(a => a.Id == audio.Id)) {
        account.RemoveFavourite(audio);
        favourited = false;
      } else {
        if (audio.IsPrivate) {
          throw new UnauthorizedException(
            Target: "AudioService::FavouriteAudioAsync",
            Message: "You can not favourite, audio is not public."
          );
        }    
        account.AddFavourite(audio);
        favourited = true;
      }
      
      await this._db.SaveChangesAsync();
      return favourited;
    }

    public async Task<IEnumerable<AudioResponse>> ListFavouritesAync() {
      var account = await this._currentAccount.GetAsync();
      var favourites = (account.Favourites ?? []).Select(a => AudioResponse.From(a));
      return favourites.ToList();
    }
  }
}
