using AudioArchive.Database;
using Microsoft.AspNetCore.Mvc;
using AudioArchive.Infrastructure.Caching;
using AudioArchive.Modules.Artists.Services;
using AudioArchive.Infrastructure.Identity;

namespace AudioArchive.Modules.Artists.Controllers
{
  [ApiController]
  [Route("api/artist")]
  public partial class ArtistController(
    IArtistService artistService,
    ICachingService cachingService,
    DatabaseContext databaseContext,
    ICurrentAccount currentAccout
  ) : ControllerBase
  {
    private readonly IArtistService _artistService = artistService;
    private readonly ICurrentAccount _currentAccount = currentAccout;
    private readonly ICachingService _cachingService = cachingService;
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly string CacheGroup = "artist";
  }
}
