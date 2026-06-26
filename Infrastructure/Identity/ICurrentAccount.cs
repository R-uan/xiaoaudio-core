using AudioArchive.Database.Entity;
namespace AudioArchive.Infrastructure.Identity
{
  public interface ICurrentAccount
  {
    Task<Account> GetAsync();
  }
}