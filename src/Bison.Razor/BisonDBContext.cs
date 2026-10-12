namespace Bison;

using Microsoft.EntityFrameworkCore;

public class BisonDBContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Taxon> Taxons { get; set; }

    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<long>("PostIds");
        
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.AuthorId);
            entity.Property(a => a.Name).IsRequired();
            entity.Property(a => a.Email).IsRequired();
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.UseTpcMappingStrategy();
            entity.HasKey(p => p.PostId);
            entity.Property(p => p.PostId).HasDefaultValueSql("NEXT VALUE FOR PostIds");
            entity.HasOne(p => p.Author).WithMany(a => a.Posts).IsRequired();
            entity.Property(p => p.Text).IsRequired().HasMaxLength(220);
            entity.Property(p => p.Timestamp).IsRequired();
        });

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.Property(o => o.Location).IsRequired();
            entity.HasOne(o => o.Taxon).WithMany().IsRequired();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(c => c.Observation).WithMany().IsRequired();
        });

        modelBuilder.Entity<Proposal>(entity =>
        {
            entity.HasOne(p => p.Observation).WithMany().IsRequired();
            entity.HasOne(p => p.Taxon).WithMany().IsRequired();
        });

        modelBuilder.Entity<Taxon>(entity =>
        {
            entity.HasKey(t => t.TaxonId);
            entity.Property(t => t.dwc_TaxonId).IsRequired();
            entity.HasOne(t => t.Parent).WithMany(t => t.Children).IsRequired(false);
        });
    }
}