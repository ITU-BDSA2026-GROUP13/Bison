namespace Bison.Tests.IntegrationTests;

/*
 * A small, predictable data set for the repository integration tests.
 *
 * Authors:  AuthorA has 35 observations (Heron)          -> paging: 32 + 3
 *           AuthorB has 3 observations (2 Swan, 1 Unnamed)
 *           AuthorC has no observations
 * Taxa:     Heron and Swan have a Danish name, Unnamed does not
 * Comments: 2 on ObservationWithComments, 1 on OtherObservation
 * Proposals: 1 on ObservationWithComments (Unnamed taxon), 1 on OtherObservation
 *
 * The posts are saved in three rounds so that the comments get PostIds
 * in between observation ids, and the timestamps of AuthorA's observations
 * are not in insertion order, so a missing orderby is noticed.
 */
public static class TestDbInitializer
{
    public const int AuthorAObservationCount = 35;

    public static TestData Seed(BisonDBContext context)
    {
        var baseTime = new DateTime(2026, 1, 1, 12, 0, 0);

        var authorA = new Author { UserName = "AuthorA", Email = "a@test.dk" };
        var authorB = new Author { UserName = "AuthorB", Email = "b@test.dk" };
        var authorC = new Author { UserName = "AuthorC", Email = "c@test.dk" };

        var heron = new Taxon { dwc_TaxonID = "TEST:heron", VernacularName = "Fiskehejre" };
        var swan = new Taxon { dwc_TaxonID = "TEST:swan", VernacularName = "Knopsvane" };
        var unnamed = new Taxon { dwc_TaxonID = "TEST:unnamed", VernacularName = null };

        context.Authors.AddRange(authorA, authorB, authorC);
        context.Taxons.AddRange(heron, swan, unnamed);

        // Round 1: AuthorB's observations
        var observationWithComments = new Observation
        {
            Author = authorB, Taxon = swan, Text = "Two swans on the lake",
            Location = "Lake", TimeStamp = baseTime.AddDays(-1)
        };
        var otherObservation = new Observation
        {
            Author = authorB, Taxon = swan, Text = "A swan in the harbour",
            Location = "Harbour", TimeStamp = baseTime.AddDays(-2)
        };
        var unnamedObservation = new Observation
        {
            Author = authorB, Taxon = unnamed, Text = "A bird I could not name",
            Location = "Forest", TimeStamp = baseTime.AddDays(-3)
        };
        context.Observations.AddRange(observationWithComments, otherObservation, unnamedObservation);
        context.SaveChanges();

        // Round 2: comments and proposals, so their PostIds come before AuthorA's observations
        var comments = new List<Comment>
        {
            new Comment { Author = authorA, Observation = observationWithComments, Text = "Nice find!", TimeStamp = baseTime.AddHours(1) },
            new Comment { Author = authorC, Observation = observationWithComments, Text = "Great photo", TimeStamp = baseTime.AddHours(2) },
            new Comment { Author = authorA, Observation = otherObservation, Text = "Same swan as last week?", TimeStamp = baseTime.AddHours(3) }
        };
        var proposals = new List<Proposal>
        {
            new Proposal { Author = authorC, Observation = observationWithComments, Taxon = unnamed, Text = "Maybe this one?", TimeStamp = baseTime.AddHours(4) },
            new Proposal { Author = authorA, Observation = otherObservation, Taxon = heron, Text = "Looks like a heron", TimeStamp = baseTime.AddHours(5) }
        };
        context.Comments.AddRange(comments);
        context.Proposals.AddRange(proposals);
        context.SaveChanges();

        // Round 3: AuthorA's observations for paging.
        // (i * 11) % 35 gives each observation a different hour, in a shuffled order.
        var authorAObservations = new List<Observation>();
        for (int i = 0; i < AuthorAObservationCount; i++)
        {
            authorAObservations.Add(new Observation
            {
                Author = authorA, Taxon = heron, Text = $"Heron number {i}",
                Location = "Marsh", TimeStamp = baseTime.AddHours(-(i * 11 % AuthorAObservationCount) - 10)
            });
        }
        context.Observations.AddRange(authorAObservations);
        context.SaveChanges();

        return new TestData(
            authorA, authorB, authorC,
            heron, swan, unnamed,
            observationWithComments, otherObservation, unnamedObservation,
            authorAObservations, comments, proposals);
    }
}

// The seeded objects, so tests can use their generated ids instead of hard-coded numbers.
public record TestData(
    Author AuthorA,
    Author AuthorB,
    Author AuthorC,
    Taxon Heron,
    Taxon Swan,
    Taxon Unnamed,
    Observation ObservationWithComments,
    Observation OtherObservation,
    Observation UnnamedObservation,
    List<Observation> AuthorAObservations,
    List<Comment> Comments,
    List<Proposal> Proposals);
