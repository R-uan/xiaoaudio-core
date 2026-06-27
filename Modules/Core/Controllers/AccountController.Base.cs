using Microsoft.AspNetCore.Mvc;
using AudioArchive.Modules.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using AudioArchive.Modules.Core.Services;

namespace AudioArchive.Modules.Core.Controllers {
  public partial class AccountController(
    IAccountService accountService
  ) {
    private readonly IAccountService _accountService = accountService;
  }
}