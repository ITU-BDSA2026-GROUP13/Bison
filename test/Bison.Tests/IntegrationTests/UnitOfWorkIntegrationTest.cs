namespace Bison.Tests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Bison.Repository;
using Xunit;

public class UnitOfWorkIntegrationTests : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbContextOptions<BisonDBContext> options;

    public UnitOfWorkIntegrationTests()
    {
        // In-memory SQLite only lives as long as the connection is open
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        options = new DbContextOptionsBuilder<BisonDBContext>()
            .UseSqlite(connection)
            .Options;

        using var ctx = new BisonDBContext(options);
        ctx.Database.Migrate();
    }

    public void Dispose() => connection.Dispose();

    private static Observation NewObservation(long postId, long authorId, long taxonId) => new()
    {
        PostId = postId,
        Author = new Author { AuthorId = authorId, Name = "Mikkel", Email = "msyj@itu.dk" },
        Taxon = new Taxon { TaxonId = taxonId, dwc_TaxonID = $"TEST:{taxonId}", VernacularName = "Tophejre", Parent = null },
        Text = "Der en fugl",
        Location = "Der hjemme",
        TimeStamp = DateTime.Now
    };

    [Fact]
    public async Task Save_PersistsObservation()
    {
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(1, 1, 1));
            uow.Save();
        }

        // Fresh context proves the data is in the database, not just the change tracker
        using var verify = new BisonDBContext(options);
        var result = await new ObservationRepository(verify).ReadObservation(1);

        Assert.NotNull(result);
        Assert.Equal("Mikkel", result.Author.Name);
        Assert.Equal("TEST:1", result.Taxon.dwc_TaxonID);
    }

    [Fact]
    public async Task WithoutSave_NothingIsPersisted()
    {
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(1, 1, 1));
            // No uow.Save()
        }

        using var verify = new BisonDBContext(options);
        var result = await new ObservationRepository(verify).ReadObservation(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task Save_WhenOneInsertFails_PersistsNothing()
    {
        // Arrange: PostId 1 already exists in the database
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(1, 1, 1));
            uow.Save();
        }

        // Act: one valid insert (PostId 2) and one duplicate key (PostId 1) in the same Save()
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(2, 2, 2));
            await uow.observationRepo.CreateObservation(NewObservation(1, 1, 1));

            Assert.ThrowsAny<DbUpdateException>(() => uow.Save());
        }

        // Assert: atomicity, so the valid PostId 2 must not have been saved either
        using var verify = new BisonDBContext(options);
        var repo = new ObservationRepository(verify);

        Assert.NotNull(await repo.ReadObservation(1));
        Assert.Null(await repo.ReadObservation(2));
    }

    [Fact]
    public void Save_WithNoChanges_DoesNotThrow()
    {
        using var ctx = new BisonDBContext(options);
        IUnitOfWork uow = new UnitOfWork(ctx);

        var exception = Record.Exception(() => uow.Save());

        Assert.Null(exception);
    }
}