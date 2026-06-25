using AudioArchive.Database.Entity;

namespace AudioArchive.Infrastructure.Providers {
  public class AuthenticationProvider : IAuthenticationProvider
  {
    public string GenerateToken(Account subject) {
      throw new NotImplementedException();
    }
  }
}