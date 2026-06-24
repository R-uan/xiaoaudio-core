using AudioArchive.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace AudioArchive.Database
{
  public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
  {
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Artist> Artists { get; set; }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Audio> Audios { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<ArtistSocial> ArtistSocials { get; set; }
    public DbSet<AudioMetadata> AudioMetadata { get; set; }

    public DbSet<SupportTicket> SupportTickets { get; set; }
    public DbSet<SupportTicketMessage> SupportTicketMessages { get; set; }
    public DbSet<SupportTicketAttachment> SupportTicketAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Artist>(artist => {
        artist.ToTable("artists");
        artist.HasKey(a => a.Id);
        artist.HasIndex(a => a.Name).IsUnique();
        artist.HasMany(a => a.Socials)
              .WithOne(s => s.Artist)
              .HasForeignKey(s => s.ArtistId)
              .OnDelete(DeleteBehavior.Cascade);
        artist.HasMany(a => a.Audios)
              .WithOne(a => a.Artist)
              .HasForeignKey(a => a.ArtistId)
              .OnDelete(DeleteBehavior.Cascade);
        artist.HasOne(a => a.VerifiedAccount)
              .WithOne(u => u.ArtistProfile)
              .HasForeignKey<Artist>(a => a.VerifiedAccountId)
              .OnDelete(DeleteBehavior.SetNull);
      });
      modelBuilder.Entity<Audio>(audio => {
        audio.ToTable("audios");
        audio.HasKey(a => a.Id);
        audio.HasOne(a => a.Metadata)
             .WithOne(m => m.Audio)
             .HasForeignKey<AudioMetadata>(m => m.AudioId)
             .OnDelete(DeleteBehavior.Cascade);
      });
      modelBuilder.Entity<AudioMetadata>(metadata => {
        metadata.ToTable("audio_metadata");
        metadata.HasIndex(am => am.AudioId);
        metadata.HasKey(m => m.Id);
        metadata.HasMany(m => m.Tags)
                .WithMany(t => t.AudioMetadatas)
                .UsingEntity(j => {
                  j.ToTable("audio_metadata_tags");
                  j.HasIndex("TagsId");
                });
      });
      modelBuilder.Entity<Tag>(tag => {
        tag.ToTable("tags");
        tag.HasKey(t => t.Id);
        tag.HasIndex(t => t.Name).IsUnique();
      });
      modelBuilder.Entity<Playlist>(playlist => {
        playlist.ToTable("playlists");
        playlist.HasMany(p => p.Audios)
                .WithMany(a => a.Playlists)
                .UsingEntity(j => {
                  j.ToTable("playlist_audios");
                });
      });
      modelBuilder.Entity<ArtistSocial>(socials => {
        socials.ToTable("artist_socials");
        socials.HasOne(s => s.Artist)
               .WithMany(a => a.Socials);
      });
      modelBuilder.Entity<Account>(user => {
        user.ToTable("users");
        user.HasKey(u => u.Id);
        user.HasIndex(u => u.Email).IsUnique();
        user.HasIndex(u => u.Username).IsUnique();
        user.HasMany(u => u.Favourites)
            .WithMany()
            .UsingEntity(j => j.ToTable("user_favourite_audios"));
      });
      modelBuilder.Entity<SupportTicket>(ticket => {
        ticket.ToTable("support_tickets");
        ticket.HasKey(t => t.Id);
        ticket.Property(t => t.Type)
              .HasConversion<string>();
        ticket.Property(t => t.Status)
              .HasConversion<string>();
        ticket.HasOne(t => t.Requester)
              .WithMany()
              .HasForeignKey(t => t.RequesterId)
              .OnDelete(DeleteBehavior.Restrict);
        ticket.HasOne(t => t.Representative)
              .WithMany()
              .HasForeignKey(t => t.RepresentativeId)
              .OnDelete(DeleteBehavior.SetNull);
        ticket.HasMany(t => t.Attachments)
              .WithOne(a => a.SupportTicket)
              .HasForeignKey(a => a.SupportTicketId)
              .OnDelete(DeleteBehavior.Cascade);
        ticket.HasMany(t => t.Messages)
              .WithOne(m => m.SupportTicket)
              .HasForeignKey(m => m.SupportTicketId)
              .OnDelete(DeleteBehavior.Cascade);
      });
      modelBuilder.Entity<SupportTicketAttachment>(attachment => {
        attachment.ToTable("support_ticket_attachments");
        attachment.HasKey(a => a.Id);
      });
      modelBuilder.Entity<SupportTicketMessage>(message => {
        message.ToTable("support_ticket_messages");
        message.HasKey(m => m.Id);
        message.HasOne(m => m.User)
               .WithMany()
               .HasForeignKey(m => m.UserId)
               .OnDelete(DeleteBehavior.Restrict);
      });
      base.OnModelCreating(modelBuilder);
    }
  }
}
