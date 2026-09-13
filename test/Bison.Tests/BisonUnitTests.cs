namespace Bison.Tests;

using Bison;
using SimpleDB;

public class BisonUnitTests
{
    [Fact]
    public void TestAddCommentStoresCommentForExistingObservation()
    {
        var cheeps = new TestDatabase<Cheep>();
        var comments = new TestDatabase<Comment>();
        cheeps.Store(new Cheep(69, "Lars", "test", 1000));

        Program.addComment(69, "comment", cheeps, comments);

        List<Comment> storedComments = comments.Read().ToList();
        Assert.Single(comments.Read());
        Assert.Equal(69, storedComments[0].CheepID);
        Assert.Equal("comment", storedComments[0].Message);
    }

    [Fact]
    public void TestAddCommentThrowsForNonExistingObservation()
    {
        var cheeps = new TestDatabase<Cheep>();
        var comments = new TestDatabase<Comment>();

        Assert.Throws<InvalidOperationException>(() =>
            Program.addComment(69, "comment", cheeps, comments));

        Assert.Empty(comments.Read());
    }


    //Så dette er vores egen lille database her som implementerer vores interface jo
    //DET FAKTISK SÅ SMART OMG!!!
    //FIK EN LATE NIGHT ÅBENBARING!!!!
    private class TestDatabase<T> : IDatabaseRepository<T>
    {
        private readonly List<T> records = [];

        public IEnumerable<T> Read(int? limit = null)
        {
            return records;
        }

        public void Store(T record)
        {
            records.Add(record);
        }
    }
}