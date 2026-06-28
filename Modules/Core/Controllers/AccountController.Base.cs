using AudioArchive.Database;
using AudioArchive.Modules.Core.Services;
using AudioArchive.Infrastructure.Identity;

using Microsoft.AspNetCore.Mvc;

namespace AudioArchive.Modules.Core.Controllers
{
  [ApiController]
  [Route("api/account")]
  public partial class AccountController(
    IAccountService accountService,
    ICurrentAccount currentAccout,
    DatabaseContext databaseContext
  ) : ControllerBase
  {
    private readonly IAccountService _accountService = accountService;
    private readonly ICurrentAccount _currentAccount = currentAccout;
    private readonly DatabaseContext _db = databaseContext;
  }
}
