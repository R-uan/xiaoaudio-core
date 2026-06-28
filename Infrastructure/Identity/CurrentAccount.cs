using AudioArchive.Database;
using System.Security.Claims;
using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace AudioArchive.Infrastructure.Identity
{
  public class CurrentAccount : ICurrentAccount
  {
    private readonly IHttpContextAccessor _http;
    private readonly DatabaseContext _db;
    private Account? _cached;

    public CurrentAccount(IHttpContextAccessor http, DatabaseContext db) {
      _http = http;
      _db = db;
    }

    public async Task<Account> GetAsync() {
      if (_cached is not null) return _cached;

      var id = _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("Not authenticated.");

      if (!int.TryParse(id, out var accountId))
        throw new UnauthorizedAccessException($"Invalid subject claim: '{id}'");

        _cached = await _db.Accounts
          .Where(a => a.Id == accountId)
          .Include(a => a.Favourites)
            .ThenInclude(f => f.Metadata)  // f is Audio here, not Account
          .Include(a => a.Permissions)
          .FirstOrDefaultAsync() ?? 
          throw new UnauthorizedAccessException("Account not found.");

      return _cached;
    }
  }
}