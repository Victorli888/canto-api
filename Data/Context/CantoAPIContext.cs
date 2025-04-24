namespace Data.Context;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tell EF Core to use lowercase table names
        modelBuilder.Entity<Phrase>()
            .ToTable("phrases");

        modelBuilder.Entity<Theme>()
            .ToTable("themes");

        modelBuilder.Entity<Phrase>(entity =>
        {
            entity.ToTable("phrases");
            
            // Map C# property names to database property names
            entity.Property(e => e.PhraseId).HasColumnName("phrase_id");
            entity.Property(e => e.Cantonese).HasColumnName("cantonese");
            entity.Property(e => e.English).HasColumnName("english");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");
            entity.Property(e => e.ChallengeRating).HasColumnName("challenge_rating");
            entity.Property(e => e.RootQuestionId).HasColumnName("root_question_id");
            entity.Property(e => e.IsHidden).HasColumnName("is_hidden");

            //TODO: Property mapping will be requried for themes table, We could use EF Naming Conventions instead.
        });
    }
}