using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Services;
using AudioArchive.Infrastructure.Identity;

namespace AudioArchive.Modules.Core.Controllers
{
  [ApiController]
  [Route("api/account")]
  public partial class AccountController(
    IAccountService accountService,
    ICurrentAccount currentAccout
  ) : ControllerBase
  {
    private readonly IAccountService _accountService = accountService;
    private readonly ICurrentAccount _currentAccount = currentAccout;
  }
}
