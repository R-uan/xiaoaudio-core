using AudioArchive.Shared;

namespace AudioArchive.Modules.Audios.Services
{
  public partial class AudioService : IAudioService
  {
    public async Task<bool> FavouriteAudioAsync(Guid audioGuid) {
      var account = await this._currentAccount.GetAsync()!;
      
      var audio = await this._databaseContext.Audios.FindAsync(audioGuid) ??
        throw new NotFoundException(
          Message: "Audio was not found.",
          Target: "AudioService::FavouriteAudioAsync"
        );

      if (audio.IsPrivate) {
        throw new UnauthorizedException(
          Target: "AudioService::FavouriteAudioAsync",
          Message: "You can not favourite, audio is not public."
        );
      }
      
      account.AddFavourite(audio);
      await this._databaseContext.SaveChangesAsync();
      
      return true;
    }
  }
}
