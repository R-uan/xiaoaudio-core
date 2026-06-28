namespace AudioArchive.Database.Entity
{
  public class Permission
  {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<Account> Accounts { get; set; } = [];
  }
}
