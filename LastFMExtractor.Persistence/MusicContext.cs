using LastFMExtractor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LastFMExtractor.Persistence
{
    public partial class MusicContext : DbContext
    {
        public MusicContext()
        {
        }

        public MusicContext(DbContextOptions<MusicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ExportedTracks> ExportedTracks { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobFailure> JobFailures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=HONDURAS;Database=Music;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");
            
            modelBuilder.Entity<ExportedTracks>(entity =>
            {
                entity.ToTable("exported_tracks");

                entity.Property(e => e.Album).IsUnicode(false);

                entity.Property(e => e.AlbumId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Artist).IsUnicode(false);

                entity.Property(e => e.ArtistId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");

                entity.Property(e => e.DateExtracted)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Track).IsUnicode(false);

                entity.Property(e => e.TrackId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.ExportedTracks)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK__exported___JobId__5DCAEF64");
            });

            modelBuilder.Entity<JobFailure>(entity =>
            {
                entity.HasKey(e => e.FailureId)
                    .HasName("PK__JobFailu__60D392718578A706");

                entity.Property(e => e.ExceptionDetails).IsUnicode(false);                
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.Property(e => e.JobId).ValueGeneratedNever();

                entity.Property(e => e.EndDateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.StartDateTime).HasColumnType("smalldatetime");
            });
        }
    }
}