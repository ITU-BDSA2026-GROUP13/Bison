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
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.AuthorId);
            entity.Property(a => a.Name).IsRequired();
            entity.Property(a => a.Email).IsRequired();
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.UseTptMappingStrategy();
            entity.HasKey(p => p.PostId);
            entity.HasOne(p => p.Author).WithMany(a => a.Posts).IsRequired();
            entity.Property(p => p.Text).IsRequired().HasMaxLength(220);
            entity.Property(p => p.TimeStamp).IsRequired();
        });

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.Property(o => o.Location).IsRequired(false);
            entity.HasOne(o => o.Taxon).WithMany().IsRequired();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(c => c.Observation).WithMany(o => o.Comments).IsRequired();
        });

        modelBuilder.Entity<Proposal>(entity =>
        {
            entity.HasOne(p => p.Observation).WithMany(o => o.Proposals).IsRequired();
            entity.HasOne(p => p.Taxon).WithMany().IsRequired();
        });

        modelBuilder.Entity<Taxon>(entity =>
        {
            entity.HasKey(t => t.TaxonId);
            entity.Property(t => t.dwc_TaxonID).IsRequired();
            entity.HasOne(t => t.Parent).WithMany(t => t.Children).IsRequired(false);
        });
    }
}