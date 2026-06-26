using AudioArchive.Database;
using AudioArchive.Infrastructure.Identity;

namespace AudioArchive.Modules.Audios.Services
{
  public partial class AudioService(
    DatabaseContext databaseContext,
    ICurrentAccount currentAccout
  ) : IAudioService
  {
    private readonly ICurrentAccount _currentAccount = currentAccout;
    private readonly DatabaseContext _databaseContext = databaseContext;
  }
}
