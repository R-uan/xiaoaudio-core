using Microsoft.AspNetCore.Authorization;

namespace AudioArchive.Infrastructure.Identity
{
  public record PermissionRequirement(string Permission) : IAuthorizationRequirement;

  public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
  {
    private readonly ICurrentAccount _currentAccount;

    public PermissionHandler(ICurrentAccount currentAccount) {
      _currentAccount = currentAccount;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement) {
      var account = await _currentAccount.GetAsync();

      if (account.Permissions.Any(p => p.Name == requirement.Permission))
        context.Succeed(requirement);
    }
  }
}