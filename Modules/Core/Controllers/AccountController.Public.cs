using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Requests;

namespace AudioArchive.Modules.Core.Controllers
{
  public partial class AccountController
  {
    [HttpPost("authentication")]
    public async Task<IActionResult> AuthenticateAccount([FromBody] AuthenticationRequest request) {
      var token = await _accountService.AuthenticateAccountAsync(request);
      return Ok(new { Success = true, Data = new { Token = token } });
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request) {
      var account = await _accountService.RegisterAccountAsync(request);
      return Ok(new { Success = true, Data = account });
    }

    [HttpPost("verification")]
    public async Task<IActionResult> VerifyUnique([FromQuery] string type, [FromQuery] string value) {
      if (type.ToLower() == "email") {
        return Ok(new {
          Success = true,
          Available = await _accountService.VerifyEmailAvailabilityAsync(value),
        });
      } else if (type.ToLower() == "username") {
        return Ok(new {
          Success = true,
          Available = await _accountService.VerifyUsernameAvailabilityAsync(value),
        });
      } else {
        return BadRequest(new { Success = false, Message = "Invalid value." });
      }
    }
  }
}
