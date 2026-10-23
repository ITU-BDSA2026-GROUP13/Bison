namespace Bison.Tests;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Bison.Repository;
using Bison.Tests.IntegrationTests;
using Xunit;

public class UnitOfWorkIntegrationTests : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbContextOptions<BisonDBContext> options;
    private readonly TestData data;

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
        data = TestDbInitializer.Seed(ctx);
    }

    public void Dispose() => connection.Dispose();

    // Reuses the seeded author and taxon via their ids, so no duplicate rows are inserted
    private Observation NewObservation(BisonDBContext ctx,long postId) => new()
    {
        PostId = postId,
        Author = ctx.Authors.Find(data.AuthorC.AuthorId)!,
        Taxon = ctx.Taxons.Find(data.Swan.TaxonId)!,
        Text = "A new bird",
        Location = "Home",
        TimeStamp = DateTime.Now
    };

    [Fact]
    public async Task Save_PersistsObservation()
    {
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(ctx, 9001));
            uow.Save();
        }

        // Fresh context proves the data is in the database, not just the change tracker
        using var verify = new BisonDBContext(options);
        var result = await new ObservationRepository(verify).ReadObservation(9001);

        Assert.NotNull(result);
        Assert.Equal("AuthorC", result.Author.Name);
        Assert.Equal("TEST:swan", result.Taxon.dwc_TaxonID);
    }

    [Fact]
    public async Task WithoutSave_NothingIsPersisted()
    {
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(ctx, 9001));
            // No uow.Save()
        }

        using var verify = new BisonDBContext(options);
        var result = await new ObservationRepository(verify).ReadObservation(9001);

        Assert.Null(result);
    }

    [Fact]
    public async Task Save_WhenOneInsertFails_PersistsNothing()
    {
        var existingId = data.OtherObservation.PostId; // already seeded, so inserting it again fails

        // Act: one valid insert (9001) and one duplicate key in the same Save()
        using (var ctx = new BisonDBContext(options))
        {
            IUnitOfWork uow = new UnitOfWork(ctx);
            await uow.observationRepo.CreateObservation(NewObservation(ctx, 9001));
            await uow.observationRepo.CreateObservation(NewObservation(ctx, existingId));

            Assert.ThrowsAny<DbUpdateException>(() => uow.Save());
        }

        // Assert: atomicity, so the valid 9001 must not have been saved either
        using var verify = new BisonDBContext(options);
        var repo = new ObservationRepository(verify);

        Assert.NotNull(await repo.ReadObservation(existingId));
        Assert.Null(await repo.ReadObservation(9001));
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