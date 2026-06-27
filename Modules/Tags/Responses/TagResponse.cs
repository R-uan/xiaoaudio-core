namespace AudioArchive.Modules.Tags.Responses
{
  public class TagResponse
  {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int AudioCount { get; set; }
  }
}
