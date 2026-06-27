using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Support.Services;
using AudioArchive.Infrastructure.Identity;

namespace AudioArchive.Modules.Support.Controllers
{
  [ApiController]
  [Route("api/support")]
  public partial class SupportController(
    ISupportService supportService,
    ICurrentAccount currentAccout
  ) : ControllerBase
  {
    private readonly ISupportService _supportService = supportService;
    private readonly ICurrentAccount _currentAccount = currentAccout;
  }
}
