using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Request;
using AudioArchive.Modules.Core.Services;

namespace AudioArchive.Modules.Core.Controller {
  public partial class AccountController(IAccountService accountService) : ControllerBase {
    private readonly IAccountService _accountService = accountService;
    
    public async Task<IActionResult> AuthenticateAccount([FromBody] AuthenticationRequest request) {
      var token = await this._accountService.AuthenticateAccount(request);
      return Ok(new {
        Success = true,
        Data = new { Token = token }
      });
    }

    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request) {
      var account = this._accountService.RegisterAccountAsync(request);
      return Ok(new {
        Success = true,
        Date = account
      });
    }
  }
}