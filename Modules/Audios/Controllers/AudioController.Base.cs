using AudioArchive.Database;
using Microsoft.AspNetCore.Mvc;
using AudioArchive.Infrastructure.Caching;
using AudioArchive.Modules.Audios.Services;
using AudioArchive.Infrastructure.Identity;

namespace AudioArchive.Modules.Audios.Controllers
{
  [ApiController]
  [Route("api/audio")]
  public partial class AudioController(
    IAudioService audioService,
    ICachingService cachingService,
    DatabaseContext databaseContext,
    ICurrentAccount currentAccout
  ) : ControllerBase
  {
    private readonly IAudioService _audioService = audioService;
    private readonly ICurrentAccount _currentAccount = currentAccout;
    private readonly ICachingService _cachingService = cachingService;
    private readonly DatabaseContext _databaseContext = databaseContext;

    private readonly string CacheGroup = "audio";
  }
}
