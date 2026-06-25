using AudioArchive.Database.Entity;
namespace AudioArchive.Infrastructure.Providers {
  public interface IAuthenticationProvider {
    string GenerateToken(Account subject);
  }
}