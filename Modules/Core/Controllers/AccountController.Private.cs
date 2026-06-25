using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Requests;
using Microsoft.AspNetCore.Authorization;

namespace AudioArchive.Modules.Core.Controllers {
  public partial class AccountController {
    [Authorize]
    public async Task<IActionResult> PasswordChangeAction([FromBody] PasswordChangeRequest req) {
      await this._accountService.PasswordChangeAsync(req);
      return Ok(new { Success = true });
    }
  }
}