using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Requests;

namespace AudioArchive.Modules.Core.Controllers
{
  public partial class AccountController
  {
    [HttpPost("authentication")]
    public async Task<IActionResult> AuthenticateAccount([FromBody] SignInRequest request) {
      var token = await this._accountService.SignInAsync(request);
      return Ok(new { Success = true, Data = new { Token = token } });
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegisterAccount([FromBody] SignUpRequest request) {
      var token = await this._accountService.SignUpAsync(request);
      return Ok(new { Success = true, Data = new { Token = token } });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordRequest([FromBody] ForgotPasswordRequest req) {
      return this.Ok(new {Success = await this._accountService.ForgotPasswordAsync(req)});
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ChangeForgottenPassword([FromBody] ResetPasswordRequest req) {
      return this.Ok(new {Success = await this._accountService.ResetPasswordAsync(req)});
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
