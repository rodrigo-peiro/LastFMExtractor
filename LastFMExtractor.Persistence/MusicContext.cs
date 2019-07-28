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

            //modelBuilder.Entity<ExportedTracks>(entity =>
            //{
            //    entity.ToTable("exported_tracks");

            //    entity.Property(e => e.Album).IsUnicode(false);

            //    entity.Property(e => e.AlbumId)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Artist).IsUnicode(false);

            //    entity.Property(e => e.ArtistId)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");
            //    entity.Property(e => e.DateExtracted).HasColumnType("smalldatetime").HasDefaultValueSql("getdate()");

            //    entity.Property(e => e.Track).IsUnicode(false);

            //    entity.Property(e => e.TrackId)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.JobId).HasColumnType("uniqueidentifier");

            //    entity.Metadata.FindNavigation(nameof(Domain.Entities.ExportedTracks.Job)).SetPropertyAccessMode(PropertyAccessMode.Field);                
            //});

            //modelBuilder.Entity<Job>().HasKey(e => new { e.JobId });
            //modelBuilder.Entity<Job>(entity =>
            //{
            //    entity.ToTable("Jobs");
            //    entity.Property(e => e.JobId).HasColumnType("uniqueidentifier");
            //    entity.Property(e => e.StartDateTime).HasColumnType("smalldatetime");
            //    entity.Property(e => e.EndDateTime).HasColumnType("smalldatetime");
            //    entity.Property(e => e.RecordsProcessed).HasColumnType("int");
            //    entity.Property(e => e.RecordsFound).HasColumnType("int");
            //    entity.Property(e => e.Succeeded).HasColumnType("bit");

            //    entity.Metadata.FindNavigation(nameof(Job.ExportedTracks)).SetPropertyAccessMode(PropertyAccessMode.Field);
            //    entity.HasMany(e => e.ExportedTracks).WithOne().HasForeignKey(e => e.JobId);
            //});

            //modelBuilder.Entity<JobFailure>().HasKey(e => new { e.FailureId });
            //modelBuilder.Entity<JobFailure>(entity =>
            //{
            //    entity.ToTable("JobFailures");
            //    entity.Property(e => e.FailureId).HasColumnType("bigint");
            //    entity.Property(e => e.JobId).HasColumnType("uniqueidentifier");
            //    entity.Property(e => e.ExceptionDetails).HasColumnType("varchar(max)");

            //    entity.Metadata.FindNavigation(nameof(Job)).SetPropertyAccessMode(PropertyAccessMode.Field);
            //    entity.HasOne(e => e.Job).WithOne(f => f.JobFailure).HasConstraintName("FK_JobFailure");
            //});

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