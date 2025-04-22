namespace Data.Context
{
    using Microsoft.EntityFrameworkCore;
    using Models.Domain;
    public class CantoApiContext : DbContext
    {
        public CantoApiContext(DbContextOptions<CantoApiContext> options)
            : base(options)
        {
        }

        public DbSet<Phrase> Phrases { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // New in EF Core 9.0: Global type configurations
            configurationBuilder
                .Properties<string>()
                .HaveMaxLength(1000);

            configurationBuilder
                .Properties<decimal>()
                .HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Phrase>(entity =>
            {
                entity.HasKey(e => e.PhraseId);
                entity.Property(e => e.Cantonese).IsRequired();
                entity.Property(e => e.English).IsRequired();
                
                entity.HasOne(e => e.Theme)
                      .WithMany(e => e.Phrases)
                      .HasForeignKey(e => e.ThemeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // New in EF Core 9.0: Better index configuration
                entity.HasIndex(e => e.Cantonese)
                      .HasDatabaseName("IX_Phrases_ChineseTranslation");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.HasKey(e => e.ThemeId);
                entity.Property(e => e.ThemeName).IsRequired();
            });
        }
    }
}