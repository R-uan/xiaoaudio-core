namespace AudioArchive.Database.Entity {
  public class LoginLocation {
    public int Id { get; set; }
    public required string IP { get; set; }
    public required string Device { get; set; }
    public required string Location { get; set; }
    public DateTime LoggedAt { get; set; }

    public required int AccountId { get; set; }
    public required Account Account { get; set; }

    public bool CompareIP(string ip) {
      return this.IP == ip;
    }

    public bool CompareLocation(string location) {
      return this.Location == location;
    }

    public bool CompareDevice(string device) {
      return this.Device == device;
    }
  }
}